using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System.Linq;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using System.Web;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Amigo.Tenant.Common;
using System.IO;
using System.Net.Http;
using System.Net;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class DriverReportApplicationService : IDriverReportApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<DriverPayReportDTO> _serviceDriverReportDataAccess;
        private readonly IQueryDataAccess<AmigoTenanttServiceLatestDTO> _serviceAmigoTenantTServiceLatestDataAccess;
        private readonly IIdentitySeverAgent _repoIdentitySeverAgent;

        public ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }

        public DriverReportApplicationService(IBus bus,
           IQueryDataAccess<DriverPayReportDTO> serviceDriverReportDataAccess,
           IQueryDataAccess<AmigoTenanttServiceLatestDTO> serviceAmigoTenantTServiceLatestDataAccess,
           IMapper mapper, IIdentitySeverAgent repoIdentitySeverAgent)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _serviceDriverReportDataAccess = serviceDriverReportDataAccess;
            _serviceAmigoTenantTServiceLatestDataAccess = serviceAmigoTenantTServiceLatestDataAccess;
            _mapper = mapper;
            _repoIdentitySeverAgent = repoIdentitySeverAgent;
        }

        public async Task<ResponseDTO<PagedList<DriverPayReportDTO>>> SearchDriverPayReportAsync(DriverPayReportSearchRequest search)
        {
            Expression<Func<DriverPayReportDTO, bool>> queryFilter = p => true;

            if (search.ReportDateFrom.HasValue && search.ReportDateTo.HasValue)
            {
                var toPlusADay = search.ReportDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ReportDate.Value >= search.ReportDateFrom);
                queryFilter = queryFilter.And(p => p.ReportDate.Value < toPlusADay);
            }
            else if (search.ReportDateFrom.HasValue && !search.ReportDateTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ReportDate.Value >= search.ReportDateFrom);
            }
            else if (!search.ReportDateFrom.HasValue && search.ReportDateTo.HasValue)
            {
                var toPlusADay = search.ReportDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ReportDate.Value < toPlusADay);
            }

            if (search.DriverId > 0)
                queryFilter = queryFilter.And(p => p.DriverUserId == search.DriverId);

            if (search.DedicatedLocationId > 0)
                queryFilter = queryFilter.And(p => p.DedicatedLocationId == search.DedicatedLocationId);

            var report = await _serviceDriverReportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            ResponseDTO<List<UserResponse>> users = new ResponseDTO<List<UserResponse>>();
            if (report.Items.Any())
            {
                List<string> userList = report.Items.Select(q => q.Driver).ToList();
                users = await SetUsersAdditionalInformation(userList);
            }

            var result = report.Items
                        .GroupBy(u => new
                        {
                            u.Driver,
                            u.DriverUserId,
                            u.DedicatedLocationCode
                        })
                        .Select(g =>
                                   new DriverPayReportDTO
                                   {

                                       Driver = g.Key.Driver,
                                       DriverUserId = g.Key.DriverUserId,
                                       DedicatedLocationCode = g.Key.DedicatedLocationCode,
                                       DayPayDriverTotal = g.Sum(x => x.DayPayDriverTotal),
                                   }
                             ).ToList();

            foreach (var item in result)
            {
                Expression<Func<AmigoTenanttServiceLatestDTO, bool>> expressionLatestService = p => p.AmigoTenantTUserId == item.DriverUserId;
                var latestAmigoTenantTService = (await _serviceAmigoTenantTServiceLatestDataAccess.ListAsync(expressionLatestService)).FirstOrDefault();

                if (latestAmigoTenantTService != null)
                {
                    item.ChargeNo = latestAmigoTenantTService.ChargeNo;
                    item.ChargeType = latestAmigoTenantTService.ChargeType;
                    if (latestAmigoTenantTService.ServiceFinishDate != null)
                    {
                        item.ServiceStatusOffOnDesc = Constants.ServiceStatus.Dispatched;
                    }
                    else
                    {
                        item.ServiceStatusOffOnDesc = Constants.ServiceStatus.Offline;
                    }
                }
                else
                {
                    item.ServiceStatusOffOnDesc = Constants.ServiceStatus.Offline;
                }

                item.ServiceLatestInformation = string.Format("Current Location: {0}, Status: {1}, Dispatcher: {2}", item.CurrentLocationCode, item.ServiceStatusOffOnDesc, item.DispatcherCode);

                var user = users.Data.FirstOrDefault(q => q.UserName.ToLower() == item.Driver.ToLower());
                if (user != null)
                {
                    item.FirstName = user.FirstName;
                    item.LastName = user.LastName;
                }
            }

            var pagedResult = new PagedList<DriverPayReportDTO>()
            {
                Items = result,
                PageSize = report.PageSize,
                Page = report.Page,
                Total = result.Count
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, DriverPayReportSearchRequest search)
        {
            var list = await SearchDriverPayReportAsync(search);
            try
            {
                if (list.Data.Items.Count > 0)
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string> {
                            "Driver ID","First Name","Last Name","Total Pay","Dedicated Location","Current Activity","Charge No"
                        };
                        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
                        foreach (var item in list.Data.Items)
                        {
                            await writer.WriteLineAsync(ProcessCellDataToReport(item));
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

        private string ProcessCellDataToReport(DriverPayReportDTO item)
        {
            var total = String.Format("{0:0.00}", item.DayPayDriverTotal);
            var totalWithFormat = total.Replace(@",", ".");
            string chargeType = item.ChargeType == "C" ? "Cost Center" : "ShipmentID";
            string textProperties = ExcelHelper.StringToCSVCell(item.Driver) + "," + ExcelHelper.StringToCSVCell(item.FirstName) + "," + ExcelHelper.StringToCSVCell(item.LastName) + "," + ExcelHelper.StringToCSVCell(totalWithFormat) + "," +
                                    ExcelHelper.StringToCSVCell(item.DedicatedLocationCode) + "," + ExcelHelper.StringToCSVCell(chargeType) + "," + ExcelHelper.StringToCSVCell(item.ChargeNo);
            return textProperties;
        }

        private async Task<ResponseDTO<List<UserResponse>>> SetUsersAdditionalInformation(List<string> userListDTO)
        {
            if (userListDTO.Any())
            {
                _repoIdentitySeverAgent.IdentityServerClientSettings = this.IdentityServerClientSettings;
                var rspUsersDetails = await _repoIdentitySeverAgent.AmigoTenantTUser_Details_IdentityServer(userListDTO);


                if (rspUsersDetails.IsSuccessStatusCode)
                {
                    var userDetailsJson = await rspUsersDetails.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var users = JsonConvert.DeserializeObject<ResponseDTO<List<UserResponse>>>(userDetailsJson);
                    return users;
                }
                else
                {
                    throw new Exception("Amigo.Tenant.Application.Services.Security - DriverReportApplicationService - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
                }
            }
            return null;
        }

        private string GetUrlParameterValues(List<string> userNames)
        {
            if (!userNames.Any())
                return "";

            var builder = new StringBuilder("?");
            var separator = string.Empty;
            foreach (var userName in userNames.Where(kvp => kvp != null))
            {
                builder.AppendFormat("{0}{1}={2}", separator, HttpUtility.UrlEncode("Usernames"), HttpUtility.UrlEncode(userName.ToString()));
                separator = "&";
            }
            return builder.ToString();
        }

    }


}
