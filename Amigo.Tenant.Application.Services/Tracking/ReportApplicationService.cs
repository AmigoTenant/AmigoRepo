using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
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

    public class ReportApplicationService : IReportApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<InternalReportDTO> _reportDataAccess;


        public ReportApplicationService(IBus bus,
            IQueryDataAccess<InternalReportDTO> reportDataAccess,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _reportDataAccess = reportDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalHistoryAsync(ReportHistoryRequest search)
        {
            var queryFilter = GetQueryFilter(search);

            var eventLogs = await _reportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            eventLogs.Items.ToList().ForEach(x =>
                                                {
                                                    x.Status = (x.ServiceFinishDate == null) ? Constants.ServiceStatus.Ongoing : Constants.ServiceStatus.Done;
                                                    x.IsHazardousLabel = (string.IsNullOrEmpty(x.IsHazardous) || x.IsHazardous == "0") ? Constants.YesNo.No : Constants.YesNo.Yes;
                                                    int? approvalStatus = null;
                                                    if (x.ServiceStatus.HasValue && x.ServiceStatus.Value)
                                                        approvalStatus = 1;
                                                    if (x.ServiceStatus.HasValue && !x.ServiceStatus.Value)
                                                        approvalStatus = 0;

                                                    x.ApprovalStatus = Constants.ApprovalStatus.FirstOrDefault(y => y.Item1 == approvalStatus).Item2;
                                                    x.ServiceStartDayName = x.ServiceStartDate?.DateTime.ToString("dddd") ?? "";
                                                    x.ServiceFinishDayName = x.ServiceFinishDate?.DateTime.ToString("dddd") ?? "";
                                                    x.ServiceTotalHours = GetServiceTotalHours(x.ServiceStartDate, x.ServiceFinishDate);
                                                });

            var pagedResult = new PagedList<InternalReportDTO>()
            {

                Items = eventLogs.Items,
                PageSize = eventLogs.PageSize,
                Page = eventLogs.Page,
                Total = eventLogs.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalCurrentAsync(ReportCurrentRequest search)
        {

            var queryFilter = GetQueryFilter(search);

            var eventLogs = await _reportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            eventLogs.Items.ToList().ForEach(x =>
            {
                x.Status = (x.ServiceFinishDate == null) ? Constants.ServiceStatus.Ongoing : Constants.ServiceStatus.Done;
                x.IsHazardousLabel = (string.IsNullOrEmpty(x.IsHazardous) || x.IsHazardous == "0") ? Constants.YesNo.No : Constants.YesNo.Yes;
                int? approvalStatus = null;
                if (x.ServiceStatus.HasValue && x.ServiceStatus.Value)
                    approvalStatus = 1;
                if (x.ServiceStatus.HasValue && !x.ServiceStatus.Value)
                    approvalStatus = 0;
                x.ApprovalStatus = Constants.ApprovalStatus.FirstOrDefault(y => y.Item1 == approvalStatus).Item2;
                x.ServiceStartDayName = x.ServiceStartDate?.DateTime.ToString("dddd") ?? "";
                x.ServiceFinishDayName = x.ServiceFinishDate?.DateTime.ToString("dddd") ?? "";
                x.ServiceTotalHours = GetServiceTotalHours(x.ServiceStartDate, x.ServiceFinishDate);

            });

            var pagedResult = new PagedList<InternalReportDTO>()
            {

                Items = eventLogs.Items,
                PageSize = eventLogs.PageSize,
                Page = eventLogs.Page,
                Total = eventLogs.Total

            };

            return ResponseBuilder.Correct(pagedResult);

        }

        public async Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalHistoryAsync(ReportHistoryRequest search)
        {

            var services = await SearchInternalHistoryAsync(search);

            var pagedResult = new PagedList<ExternalReportDTO>()
            {

                Items = services.Data.Items.Select(x => new ExternalReportDTO
                {
                    AmigoTenantTUserId = x.AmigoTenantTUserId,
                    Username = x.Username,
                    Drivername = x.Drivername,
                    Status = x.Status,
                    EquipmentNumber = x.EquipmentNumber,
                    EquipmentSizeCode = x.EquipmentSizeCode,
                    EquipmentSize = x.EquipmentSize,
                    EquipmentTypeCode = x.EquipmentTypeCode,
                    EquipmentType = x.EquipmentType,
                    ServiceCode = x.ServiceCode,
                    Service = x.Service,
                    Product = x.Product,
                    IsHazardousLabel = x.IsHazardousLabel,
                    ChargeNo = x.ChargeNo,
                    OriginBlockCode = x.OriginBlockCode,
                    OriginBlock = x.OriginBlock,
                    DestinationBlockCode = x.DestinationBlockCode,
                    DestinationBlock = x.DestinationBlock,
                    Approver = x.Approver,
                    ServiceStatus = x.ServiceStatus,
                    ApprovalStatus = x.ApprovalStatus,
                    DispatchingParty = x.DispatchingParty,
                    ServiceStartDate = x.ServiceStartDate,
                    ServiceStartDayName = x.ServiceStartDayName,
                    ServiceFinishDate = x.ServiceFinishDate,
                    ServiceFinishDayName = x.ServiceFinishDayName,
                    ServiceTotalHours = x.ServiceTotalHours,
                    CustomerBill = x.CustomerBill,
                    EquipmentStatusName = x.EquipmentStatusName,
                    DriverComments = x.DriverComments,
                    ChassisNo = x.ChassisNo
                }).ToList(),
                PageSize = services.Data.PageSize,
                Page = services.Data.Page,
                Total = services.Data.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalCurrentAsync(ReportCurrentRequest search)
        {
            var services = await SearchInternalCurrentAsync(search);

            var pagedResult = new PagedList<ExternalReportDTO>()
            {

                Items = services.Data.Items.Select(x => new ExternalReportDTO
                {
                    AmigoTenantTUserId = x.AmigoTenantTUserId,
                    Username = x.Username,
                    Drivername = x.Drivername,
                    Status = x.Status,
                    EquipmentNumber = x.EquipmentNumber,
                    EquipmentSizeCode = x.EquipmentSizeCode,
                    EquipmentSize = x.EquipmentSize,
                    EquipmentTypeCode = x.EquipmentTypeCode,
                    EquipmentType = x.EquipmentType,
                    ServiceCode = x.ServiceCode,
                    Service = x.Service,
                    Product = x.Product,
                    IsHazardousLabel = x.IsHazardousLabel,
                    ChargeNo = x.ChargeNo,
                    OriginBlockCode = x.OriginBlockCode,
                    OriginBlock = x.OriginBlock,
                    DestinationBlockCode = x.DestinationBlockCode,
                    DestinationBlock = x.DestinationBlock,
                    Approver = x.Approver,
                    ServiceStatus = x.ServiceStatus,
                    ApprovalStatus = x.ApprovalStatus,
                    DispatchingParty = x.DispatchingParty,
                    ServiceStartDate = x.ServiceStartDate,
                    ServiceStartDayName = x.ServiceStartDayName,
                    ServiceFinishDate = x.ServiceFinishDate,
                    ServiceFinishDayName = x.ServiceFinishDayName,
                    ServiceTotalHours = x.ServiceTotalHours,
                    CustomerBill = x.CustomerBill,
                    EquipmentStatusName = x.EquipmentStatusName,
                    DriverComments = x.DriverComments,
                    ChassisNo = x.ChassisNo
                }).ToList(),
                PageSize = services.Data.PageSize,
                Page = services.Data.Page,
                Total = services.Data.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }

        #region Helpers

        private Expression<Func<InternalReportDTO, bool>> GetQueryFilter(ReportHistoryRequest search)
        {
            Expression<Func<InternalReportDTO, bool>> queryFilter = p => true; //p.ServiceStartDate != null && p.ServiceFinishDate != null;

            if (search.AmigoTenantTUserId != null)
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.AmigoTenantTUserId);
            }

            if (!string.IsNullOrEmpty(search.ServiceCode))
            {
                queryFilter = queryFilter.And(p => p.ServiceCode == search.ServiceCode);
            }

            if (!string.IsNullOrEmpty(search.EquipmentNumber))
            {
                queryFilter = queryFilter.And(p => p.EquipmentNumber.Contains(search.EquipmentNumber));
            }

            if (!string.IsNullOrEmpty(search.EquipmentSizeCode))
            {
                queryFilter = queryFilter.And(p => p.EquipmentSizeCode == search.EquipmentSizeCode);
            }

            if (!string.IsNullOrEmpty(search.EquipmentTypeCode))
            {
                queryFilter = queryFilter.And(p => p.EquipmentTypeCode == search.EquipmentTypeCode);
            }

            if (!string.IsNullOrEmpty(search.Approver))
            {
                queryFilter = queryFilter.And(p => p.Approver.Contains(search.Approver));
            }


            if (!string.IsNullOrEmpty(search.ChargeNumber))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.ChargeNumber.Trim()));
            }

            if (!string.IsNullOrEmpty(search.OriginBlockCode))
            {
                queryFilter = queryFilter.And(p => p.OriginBlockCode == search.OriginBlockCode);
            }

            if (!string.IsNullOrEmpty(search.DestinationBlockCode))
            {
                queryFilter = queryFilter.And(p => p.DestinationBlockCode == search.DestinationBlockCode);
            }

            if (search.ServiceStatus != -1)
            {
                if (search.ServiceStatus == 0)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == false);
                else if (search.ServiceStatus == 1)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == true);
                else if (search.ServiceStatus == 2)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == null);
            }

            if (search.DateFrom != null && search.DateTo != null)
            {
                var toPlusADay = search.DateTo.Value.Date.AddDays(1);
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.DateFrom.Value.Date);
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);
            }
            else if (search.DateFrom != null && search.DateTo == null)
            {
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.DateFrom.Value.Date);
            }
            else if (search.DateFrom == null && search.DateTo != null)
            {
                var toPlusADay = search.DateTo.Value.Date.AddDays(1);
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);
            }

            if (search.EquipmentStatusId.HasValue && search.EquipmentStatusId.Value > 0)
            {
                queryFilter = queryFilter.And(p => p.EquipmentStatusId == search.EquipmentStatusId);
            }

            if (search.ProductId != null)
            {
                queryFilter = queryFilter.And(p => p.ProductId == search.ProductId);
            }

            return queryFilter;
        }

        private Expression<Func<InternalReportDTO, bool>> GetQueryFilter(ReportCurrentRequest search)
        {

            Expression<Func<InternalReportDTO, bool>> queryFilter = p => p.ServiceStartDate != null && p.ServiceFinishDate == null;

            if (search.AmigoTenantTUserId != null)
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.AmigoTenantTUserId);
            }

            if (!string.IsNullOrEmpty(search.ServiceCode))
            {
                queryFilter = queryFilter.And(p => p.ServiceCode == search.ServiceCode);
            }

            if (!string.IsNullOrEmpty(search.EquipmentNumber))
            {
                queryFilter = queryFilter.And(p => p.EquipmentNumber.Contains(search.EquipmentNumber));
            }

            if (!string.IsNullOrEmpty(search.EquipmentSizeCode))
            {
                queryFilter = queryFilter.And(p => p.EquipmentSizeCode == search.EquipmentSizeCode);
            }

            if (!string.IsNullOrEmpty(search.EquipmentTypeCode))
            {
                queryFilter = queryFilter.And(p => p.EquipmentTypeCode == search.EquipmentTypeCode);
            }

            if (!string.IsNullOrEmpty(search.Approver))
            {
                queryFilter = queryFilter.And(p => p.Approver.Contains(search.Approver));
            }


            if (!string.IsNullOrEmpty(search.ChargeNumber))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.ChargeNumber.Trim()));
            }

            if (!string.IsNullOrEmpty(search.OriginBlockCode))
            {
                queryFilter = queryFilter.And(p => p.OriginBlockCode == search.OriginBlockCode);
            }

            if (!string.IsNullOrEmpty(search.DestinationBlockCode))
            {
                queryFilter = queryFilter.And(p => p.DestinationBlockCode == search.DestinationBlockCode);
            }

            if (search.EquipmentStatusId.HasValue && search.EquipmentStatusId.Value > 0)
            {
                queryFilter = queryFilter.And(p => p.EquipmentStatusId == search.EquipmentStatusId);
            }

            if (search.ServiceStatus != -1)
            {
                if (search.ServiceStatus == 0)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == false);
                else if (search.ServiceStatus == 1)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == true);
                else if (search.ServiceStatus == 2)
                    queryFilter = queryFilter.And(p => p.ServiceStatus == null);
            }

            if (search.ProductId != null)
            {
                queryFilter = queryFilter.And(p => p.ProductId == search.ProductId);
            }

            return queryFilter;
        }

        private double GetServiceTotalHours(DateTimeOffset? startDate, DateTimeOffset? finishDate)
        {
            double totalHours = 0;
            if (startDate.HasValue && finishDate.HasValue)
                totalHours = (finishDate.Value - startDate.Value).TotalHours;

            var value = totalHours - Math.Truncate(totalHours);

            if (value <= 0.25)
                totalHours = Math.Round(totalHours, 0) + 0.25;

            if (value > 0.25 && value <= 0.50)
                totalHours = Math.Round(totalHours, 0) + 0.50;

            if (value > 0.50 && value <= 0.75)
                totalHours = Math.Round(totalHours, 0) + 0.75;

            if (value > 0.75 && value <= 1)
                totalHours = Math.Round(totalHours, 0) + 1;

            return totalHours;

        }

        #endregion

        #region Export to excel
        public async void ProccessExcelToHistory(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ReportHistoryRequest search, string type, bool isExportForDow)
        {
            if (type == "internalHistoryReport")
            {
                var listIH = await SearchInternalHistoryAsync(search);
                GetStreamToInternalReport(outputStream, listIH.Data.Items, type, isExportForDow);
            }
            else if (type == "externalHistoryReport")
            {
                var listEH = await SearchExternalHistoryAsync(search);
                GetStreamToExternalReport(outputStream, listEH.Data.Items, type);
            }
        }

        public async void ProccessExcelToCurrent(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ReportCurrentRequest search, string type, bool isExportForDow)
        {
            if (type == "internalCurrentReport")
            {
                var listIC = await SearchInternalCurrentAsync(search);
                GetStreamToInternalReport(outputStream, listIC.Data.Items, type, isExportForDow);
            }
            else if (type == "externalCurrentReport")
            {
                var listEH = await SearchExternalCurrentAsync(search);
                GetStreamToExternalReport(outputStream, listEH.Data.Items, type);
            }
        }

        private async void GetStreamToInternalReport(Stream outputStream, IList<InternalReportDTO> list, string type, bool isExportToDow)
        {
            try
            {
                if (list.Count > 0)
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        if (!isExportToDow)
                            await writer.WriteLineAsync(GetHeaderDetail(type));
                        else
                            await writer.WriteLineAsync(GetHeaderDetailForDow(type));
                        foreach (var item in list)
                        {
                            if (!isExportToDow)
                                await writer.WriteLineAsync(GetRowDetail(item, item.DriverPay, type));
                            else
                                await writer.WriteLineAsync(GetRowDetailForDow(item, type));
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

        private async void GetStreamToExternalReport(Stream outputStream, IList<ExternalReportDTO> list, string type)
        {
            try
            {
                if (list.Count > 0)
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        await writer.WriteLineAsync(GetHeaderDetail(type));
                        foreach (var item in list)
                        {
                            await writer.WriteLineAsync(GetRowDetail(item, 0, type));
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

        private string GetHeaderDetail(string type)
        {
            var headers = new List<string> {
                "Driver", "Equipment", "Equipment Status", "Size", "Type", "Chassis No", "Service", "Product", "Haz", "Charge No", "Origin Block",
                "Destination Block", "Approver", "Approval Status", "Dispatching Party", "Start Date", "Start Time", "Start Day"
            };

            if (type == "internalHistoryReport" || type == "externalHistoryReport")
            {
                headers.Add("Finish Date");
                headers.Add("Finish Time");
                headers.Add("Finish Day");
                headers.Add("Total Hours");
            }

            if (type == "internalHistoryReport" || type == "internalCurrentReport")
                headers.Add("Driver Pay");

            headers.Add("Customer Billing");

            if (type == "internalHistoryReport" || type == "internalCurrentReport")
                headers.Add("Comments");

            var textHeaders = string.Empty;
            foreach (var item in headers)
            {
                textHeaders += item + ",";
            }
            return textHeaders;
        }

        private string GetRowDetail(ExternalReportDTO item, decimal driverpay, string type)
        {
            var cb = String.Format("{0:0.00}", item.CustomerBill);
            var cbWithFormat = cb.Replace(@",", ".");

            var th = String.Format("{0:0.00}", item.ServiceTotalHours);
            var thWithFormat = th.Replace(@",", ".");

            var dp = String.Format("{0:0.00}", driverpay);
            var dpWithFormat = dp.Replace(@",", ".");

            var product = !string.IsNullOrEmpty(item.Product) ? item.Product.Replace(@",", ".") : "";

            var startDate = string.Format("{0:MM/dd/yyyy,HH:mm,dddd}", item.ServiceStartDate) ?? "";
            if (string.IsNullOrEmpty(startDate))
                startDate = ",,";

            var finishDate = string.Format("{0:MM/dd/yyyy,HH:mm,dddd}", item.ServiceFinishDate) ?? "";
            if (string.IsNullOrEmpty(finishDate))
                finishDate = ",,";

            var chargeNo = item.ChargeNo;
            if ((type == "internalHistoryReport" || type == "internalCurrentReport") && item.ChargeType == Constants.ChargeTypeCode.Shipment)
                chargeNo = "\r00" + item.ChargeNo + "\r";

            string textProperties = ExcelHelper.StringToCSVCell(item.Username) + "," + ExcelHelper.StringToCSVCell(item.EquipmentNumber) + "," + ExcelHelper.StringToCSVCell(item.EquipmentStatusName) + "," + ExcelHelper.StringToCSVCell(item.EquipmentSize) + "," +
                                    ExcelHelper.StringToCSVCell(item.EquipmentType) + "," + ExcelHelper.StringToCSVCell(item.ChassisNo) + "," + ExcelHelper.StringToCSVCell(item.Service) + "," + ExcelHelper.StringToCSVCell(product) + ", " + ExcelHelper.StringToCSVCell(item.IsHazardousLabel) + "," + ExcelHelper.StringToCSVCell(chargeNo) + ", " +
                                    ExcelHelper.StringToCSVCell(item.OriginBlock) + "," +
                                    ExcelHelper.StringToCSVCell(item.DestinationBlock) + "," + ExcelHelper.StringToCSVCell(item.Approver) + "," + ExcelHelper.StringToCSVCell(item.ApprovalStatus) + "," + ExcelHelper.StringToCSVCell(item.DispatchingParty) + "," +
                                    startDate + ",";

            if (type == "internalHistoryReport" || type == "externalHistoryReport")
            {
                textProperties += finishDate + ",";
                textProperties += ExcelHelper.StringToCSVCell(thWithFormat) + ",";
            }

            if (type == "internalHistoryReport" || type == "internalCurrentReport")
                textProperties += ExcelHelper.StringToCSVCell(dpWithFormat) + ",";

            textProperties += ExcelHelper.StringToCSVCell(cbWithFormat);

            if (type == "internalHistoryReport" || type == "internalCurrentReport")
                textProperties += "," + ExcelHelper.StringToCSVCell(item.DriverComments);

            return textProperties;
        }
        #endregion

        #region Export to excel for dow

        private string GetHeaderDetailForDow(string type)
        {
            var headers = new List<string> {
                "Driver Id", "Driver Name", "Equipment Number", "Chassis Number", "Equipment Status", "Service", "Product", "Charge No", "Origin Block", "Destination Block", "Start Date", "Start Time"
            };

            if (type == "internalHistoryReport")
            {
                headers.Add("End Date");
                headers.Add("End Time");
            }

            headers.Add("Drivers Comments");

            var textHeaders = string.Empty;

            foreach (var item in headers)
            {
                textHeaders += item + ",";
            }
            return textHeaders;
        }

        private string GetRowDetailForDow(ExternalReportDTO item, string type)
        {
            var product = !string.IsNullOrEmpty(item.Product) ? item.Product.Replace(@",", ".") : "";

            var startDate = string.Format("{0:MM/dd/yyyy,HH:mm}", item.ServiceStartDate) ?? "";
            if (string.IsNullOrEmpty(startDate))
                startDate = ",";

            var finishDate = string.Format("{0:MM/dd/yyyy,HH:mm}", item.ServiceFinishDate) ?? "";
            if (string.IsNullOrEmpty(finishDate))
                finishDate = ",";

            string textProperties = ExcelHelper.StringToCSVCell(item.Username) + "," + ExcelHelper.StringToCSVCell(item.Drivername) + "," + ExcelHelper.StringToCSVCell(item.EquipmentNumber) + "," + ExcelHelper.StringToCSVCell(item.ChassisNo) + "," +
                                    ExcelHelper.StringToCSVCell(item.EquipmentStatusName) + "," + ExcelHelper.StringToCSVCell(item.Service) + "," + ExcelHelper.StringToCSVCell(product) + ", " +
                                    ExcelHelper.StringToCSVCell(item.ChargeNo) + ", " + ExcelHelper.StringToCSVCell(item.OriginBlock) + "," + ExcelHelper.StringToCSVCell(item.DestinationBlock) + "," +
                                    startDate + ",";

            if (type == "internalHistoryReport")
            {
                textProperties += finishDate + ",";
            }

            textProperties += ExcelHelper.StringToCSVCell(item.DriverComments);

            return textProperties;
        }
        #endregion

    }
}
