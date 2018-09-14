using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class ActivityEventLogApplicationService : IActivityEventLogApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ActivityEventLogDTO> _activityEventLogDataAccess;

        public ActivityEventLogApplicationService(IBus bus,
            IQueryDataAccess<ActivityEventLogDTO> activityEventLogDataAccess,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _activityEventLogDataAccess = activityEventLogDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<ActivityEventLogDTO>>> SearchActivityEventLogAll(ActivityEventLogSearchRequest search)
        {
            List<OrderExpression<ActivityEventLogDTO>> orderExpressionList = new List<OrderExpression<ActivityEventLogDTO>>();
            orderExpressionList.Add(new OrderExpression<ActivityEventLogDTO>(OrderType.Desc, p => p.ReportedActivityDate));

            var queryFilter = GetQueryFilter(search);
            var result = await _activityEventLogDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());
            var pagedResult = new PagedList<ActivityEventLogDTO>()
            {
                Items = result.Items,
                PageSize = result.PageSize,
                Page = result.Page,
                Total = result.Total
            };
            return ResponseBuilder.Correct(pagedResult);
        }

        private Expression<Func<ActivityEventLogDTO, bool>> GetQueryFilter(ActivityEventLogSearchRequest search)
        {
            Expression<Func<ActivityEventLogDTO, bool>> queryFilter = p => true;

            queryFilter = queryFilter.And(p => p.RowStatus == true);

            if (search.ActivityTypeIds != null && search.ActivityTypeIds.Count() > 0)
                queryFilter = queryFilter.And(p => search.ActivityTypeIds.Contains(p.ActivityTypeId));

            if (!string.IsNullOrEmpty(search.UserName))
                queryFilter = queryFilter.And(p => p.Username.Contains(search.UserName));

            if (!string.IsNullOrEmpty(search.ResultCode))
                queryFilter = queryFilter.And(p => p.LogType.Contains(search.ResultCode));

            if (search.ReportedActivityDateFrom.HasValue && search.ReportedActivityDateTo.HasValue)
            {
                var toPlusADay = search.ReportedActivityDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ReportedActivityDate.Value >= search.ReportedActivityDateFrom);
                queryFilter = queryFilter.And(p => p.ReportedActivityDate.Value < toPlusADay);
            }
            else if (search.ReportedActivityDateFrom.HasValue && !search.ReportedActivityDateTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ReportedActivityDate.Value >= search.ReportedActivityDateFrom);
            }
            else if (!search.ReportedActivityDateFrom.HasValue && search.ReportedActivityDateTo.HasValue)
            {
                var toPlusADay = search.ReportedActivityDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ReportedActivityDate.Value < toPlusADay);
            }

            if (!string.IsNullOrEmpty(search.chargeNumber))
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.chargeNumber));

            return queryFilter;
        }

        public async void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ActivityEventLogSearchRequest search)
        {
            var list = await SearchActivityEventLogAll(search);

            try
            {
                if (list.Data.Items.Count > 0)
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string> {
                            "Activity Name","Driver Id","Charge Number","From Block","To Block","Equipment Number","Product","Reported Activity Date","Result","Details","Location Provider"
                        };
                        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
                        foreach (var item in list.Data.Items)
                        {
                            await writer.WriteLineAsync(GetRowDetail(item));
                        }
                    }
                }
            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }

        private string GetRowDetail(ActivityEventLogDTO item)
        {
            var datetime = string.Format("{0:MM/dd/yyyy HH:mm:ss}", item.ReportedActivityDate) ?? "";
            var product = !string.IsNullOrEmpty(item.ProductName) ? item.ProductName.Replace(@",", ".") : "";
            string textProperties = ExcelHelper.StringToCSVCell(item.ActivityName) + "," + ExcelHelper.StringToCSVCell(item.Username) + "," + ExcelHelper.StringToCSVCell(item.ChargeNo) + "," + ExcelHelper.StringToCSVCell(item.OriginLocationName) + "," +
                                    ExcelHelper.StringToCSVCell(item.DestinationLocationName) + "," + ExcelHelper.StringToCSVCell(item.EquipmentNumber) + "," + ExcelHelper.StringToCSVCell(product) + "," +
                                    ExcelHelper.StringToCSVCell(datetime) + "," +
                                    ExcelHelper.StringToCSVCell(item.LogType) + "," + ExcelHelper.StringToCSVCell(item.Parameters) + "," + ExcelHelper.StringToCSVCell(item.LocationProvider);

            return textProperties;
        }
    }
}
