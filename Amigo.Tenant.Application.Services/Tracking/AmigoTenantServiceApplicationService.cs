
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Move;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using NodaTime;
using System.Linq;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Common.Extensions;
using Amigo.Tenant.Application.Services.Common;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Approve;
using System.Collections.Generic;
using Amigo.Tenant.Common;
using Amigo.Tenant.Application.DTOs.Responses.Common.Approve;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Web;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class AmigoTenantServiceApplicationService : IAmigoTenanttServiceApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AmigoTenanttServiceDTO> _amigoTenantTServiceDataAccess;
        private readonly IQueryDataAccess<AmigoTenantTServiceReportDTO> _serviceReportDataAccess;
        private readonly IQueryDataAccess<AmigoTenantTServiceApproveRateDTO> _serviceApproveRateDataAccess;
        private readonly IQueryDataAccess<AmigoTenantParameterDTO> _serviceAmigoTenantParameterDataAccess;
        private readonly IActivityTypeApplicationService _activityTypeService;
        private readonly IQueryDataAccess<CostCenterDTO> _costCenterDataAccess;
        private readonly IQueryDataAccess<AmigoTenantTEventLogPerHourDTO> _amigoTenantTEventLogPerHourDataAccess;
        private readonly IQueryDataAccess<RateDTO> _rateDataAccess;
        private readonly IQueryDataAccess<ProductDTO> _productDataAcces;
        private readonly IQueryDataAccess<LocationDTO> _locationDataAccess;

        public AmigoTenantServiceApplicationService(IBus bus,
           IQueryDataAccess<AmigoTenanttServiceDTO> amigoTenantTServiceDataAccess,
           IQueryDataAccess<AmigoTenantTServiceReportDTO> serviceReportDataAccess,
           IQueryDataAccess<AmigoTenantTServiceApproveRateDTO> serviceApproveRateDataAccess,
           IQueryDataAccess<AmigoTenantParameterDTO> serviceAmigoTenantParameterDataAccess,
           IActivityTypeApplicationService activityTypeService,
           IQueryDataAccess<CostCenterDTO> costCenterDataAccess,
           IMapper mapper,
           IQueryDataAccess<AmigoTenantTEventLogPerHourDTO> amigoTenantTEventLogPerHourDataAccess,
           IQueryDataAccess<RateDTO> rateDataAccess, IQueryDataAccess<ProductDTO> productDataAcces,
           IQueryDataAccess<LocationDTO> locationDataAccess)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _amigoTenantTServiceDataAccess = amigoTenantTServiceDataAccess;
            _serviceReportDataAccess = serviceReportDataAccess;
            _serviceApproveRateDataAccess = serviceApproveRateDataAccess;
            _serviceAmigoTenantParameterDataAccess = serviceAmigoTenantParameterDataAccess;
            _activityTypeService = activityTypeService;
            _costCenterDataAccess = costCenterDataAccess;
            _amigoTenantTEventLogPerHourDataAccess = amigoTenantTEventLogPerHourDataAccess;
            _rateDataAccess = rateDataAccess;
            _productDataAcces = productDataAcces;
            _locationDataAccess = locationDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<int>> RegisterAmigoTenanttServiceAsync(AmigoTenanttServiceDTO maintenance)
        {
            await ValidateRegisterMoveRelations(maintenance).ConfigureAwait(false);

            var amigoTenantService = await VerifyServiceOrderNoExists(maintenance.AmigoTenantTUserId, maintenance.ServiceOrderNo);

            if (amigoTenantService == null)
            {
                DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(maintenance.ServiceStartDateTZ);
                if (!string.IsNullOrEmpty(tz.Id))
                {
                    maintenance.ServiceStartDateTZ = tz.Id;
                }
                maintenance.ServiceStartDateUTC = DateTimeUTCCommon.DatetimeToDateUTC(maintenance.ServiceStartDate);

                var command = _mapper.Map<AmigoTenanttServiceDTO, RegisterAmigoTenanttServiceCommand>(maintenance);
                if (maintenance.CostCenterId.HasValue && maintenance.CostCenterId > 0)
                {
                    Expression<Func<CostCenterDTO, bool>> queryFilter = c => true;
                    queryFilter = queryFilter.And(p => p.CostCenterId == maintenance.CostCenterId);

                    var costCenter = await _costCenterDataAccess.FirstOrDefaultAsync(queryFilter);
                    command.ChargeNo = costCenter.Code;
                }
                else
                {
                    command.ChargeNo = maintenance.ShipmentID;
                }

                //Execute Command
                var resp = await _bus.SendAsync(command);

                return resp.ToResponse();
            }
            var result = new ResponseDTO<int>(true) { Data = amigoTenantService.AmigoTenantTServiceId };
            return result;
        }

        private async Task ValidateRegisterMoveRelations(AmigoTenanttServiceDTO maintenance)
        {
            if (maintenance.ProductId.HasValue)
            {
                Expression<Func<ProductDTO, bool>> queryProductFilter = p => p.RowStatus;
                queryProductFilter = queryProductFilter.And(p => p.ProductId == maintenance.ProductId);
                var product = await _productDataAcces.FirstOrDefaultAsync(queryProductFilter).ConfigureAwait(false);
                if (product == null)
                    maintenance.ProductId = null;
            }

            if (maintenance.CostCenterId.HasValue)
            {
                Expression<Func<CostCenterDTO, bool>> queryCostCenterFilter = p => p.RowStatus;
                queryCostCenterFilter = queryCostCenterFilter.And(p => p.CostCenterId == maintenance.CostCenterId);
                var costCenter = await _costCenterDataAccess.FirstOrDefaultAsync(queryCostCenterFilter).ConfigureAwait(false);
                if (costCenter == null)
                    maintenance.CostCenterId = null;
            }

            if (maintenance.OriginLocationId.HasValue)
            {
                Expression<Func<LocationDTO, bool>> queryOriginFilter = p => p.RowStatus;
                queryOriginFilter = queryOriginFilter.And(p => p.LocationId == maintenance.OriginLocationId);
                var location = await _locationDataAccess.FirstOrDefaultAsync(queryOriginFilter).ConfigureAwait(false);
                if (location == null)
                    maintenance.OriginLocationId = null;
            }
            if (maintenance.DestinationLocationId.HasValue)
            {
                Expression<Func<LocationDTO, bool>> queryDestinationFilter = p => p.RowStatus;
                queryDestinationFilter = queryDestinationFilter.And(p => p.LocationId == maintenance.DestinationLocationId);
                var location = await _locationDataAccess.FirstOrDefaultAsync(queryDestinationFilter).ConfigureAwait(false);
                if (location == null)
                    maintenance.DestinationLocationId = null;
            }
        }

        private async Task ValidateUpdateMoveRelations(UpdateAmigoTenantServiceRequest maintenance)
        {
            if (maintenance.DestinationLocationId.HasValue)
            {
                Expression<Func<LocationDTO, bool>> queryDestinationFilter = p => p.RowStatus;
                queryDestinationFilter = queryDestinationFilter.And(p => p.LocationId == maintenance.DestinationLocationId);
                var location = await _locationDataAccess.FirstOrDefaultAsync(queryDestinationFilter).ConfigureAwait(false);
                if (location == null)
                    maintenance.DestinationLocationId = null;
            }
        }

        public async Task<ResponseDTO> UpdateAmigoTenantServiceAsync(UpdateAmigoTenantServiceRequest maintenance)
        {
            await ValidateUpdateMoveRelations(maintenance).ConfigureAwait(false);

            bool needToUpdate = !(maintenance.AmigoTenantTServiceId == 0 && Guid.Empty == maintenance.ServiceOrderNo);

            if (maintenance.AmigoTenantTServiceId == 0 && Guid.Empty != maintenance.ServiceOrderNo)
            {
                var amigoTenantService = await VerifyServiceOrderNoExists(maintenance.AmigoTenantTUserId, maintenance.ServiceOrderNo);
                if (amigoTenantService != null)
                    maintenance.AmigoTenantTServiceId = amigoTenantService.AmigoTenantTServiceId;
                else
                {
                    needToUpdate = false;
                }
            }

            if (!needToUpdate)
            {
                var result = new ResponseDTO<int>(false)
                {
                    Data = 0
                };

                result.WithMessage("No records to update", "Error");

                return result;
            }

            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(maintenance.ServiceFinishDateTZ);
            if (!string.IsNullOrEmpty(tz.Id))
            {
                maintenance.ServiceFinishDateTZ = tz.Id;
            }
            maintenance.ServiceFinishDateUTC = DateTimeUTCCommon.DatetimeToDateUTC(maintenance.ServiceFinishDate);


            var command = _mapper.Map<UpdateAmigoTenantServiceRequest, UpdateAmigoTenantServiceCommand>(maintenance);
            if (maintenance.CostCenterId.HasValue && maintenance.CostCenterId > 0)
            {
                Expression<Func<CostCenterDTO, bool>> queryFilter = c => true;
                queryFilter = queryFilter.And(p => p.CostCenterId == maintenance.CostCenterId);

                var costCenter = await _costCenterDataAccess.FirstOrDefaultAsync(queryFilter);
                command.ChargeNo = costCenter.Code;
            }
            else
            {
                command.ChargeNo = maintenance.ShipmentID;
            }

            //if (command.CostCenterId.HasValue && command.CostCenterId == 0)
            //    command.CostCenterId = null;
            //Execute Command
            var resp = await _bus.SendAsync(command);

            return resp.ToResponse();
        }

        public async Task<ResponseDTO> CancelAmigoTenantServiceAsync(CancelAmigoTenantServiceRequest maintenance)
        {
            var command = _mapper.Map<CancelAmigoTenantServiceRequest, CancelAmigoTenantServiceCommand>(maintenance);
            command.RowStatus = false;
            var resp = await _bus.SendAsync(command);
            return resp.ToResponse();
        }

        public async Task<AmigoTenanttServiceDTO> VerifyServiceOrderNoExists(int? userId, Guid serviceOrderNo)
        {

            if (!userId.HasValue || userId == 0 || Guid.Empty == serviceOrderNo)
                return null;
            Expression<Func<AmigoTenanttServiceDTO, bool>> queryFilter = p => true;
            queryFilter = queryFilter.And(p => p.AmigoTenantTUserId.Value == userId);
            queryFilter = queryFilter.And(p => p.ServiceOrderNo == serviceOrderNo);

            return await _amigoTenantTServiceDataAccess.FirstOrDefaultAsync(queryFilter);
        }


        public string WindowsToIana(string windowsZoneId)
        {
            if (windowsZoneId.Equals("UTC", StringComparison.Ordinal))
                return "Etc/UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(windowsZoneId);
            if (tzi == null)
                return null;
            var tzid = tzdbSource.MapTimeZoneId(tzi);
            if (tzid == null)
                return null;
            return tzdbSource.CanonicalIdMap[tzid];
        }

        public string IanaToWindows(string ianaZoneId)
        {
            var utcZones = new[] { "Etc/UTC", "Etc/UCT", "Etc/GMT" };
            if (utcZones.Contains(ianaZoneId, StringComparer.Ordinal))
                return "UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // resolve any link, since the CLDR doesn't necessarily use canonical IDs
            var links = tzdbSource.CanonicalIdMap
                .Where(x => x.Value.Equals(ianaZoneId, StringComparison.Ordinal))
                .Select(x => x.Key);

            // resolve canonical zones, and include original zone as well
            var possibleZones = tzdbSource.CanonicalIdMap.ContainsKey(ianaZoneId)
                ? links.Concat(new[] { tzdbSource.CanonicalIdMap[ianaZoneId], ianaZoneId })
                : links;

            // map the windows zone
            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(possibleZones.Contains));
            if (item == null)
                return null;
            return item.WindowsId;
        }

        //APPROVE
        //APPROVE
        //APPROVE
        public async Task<ResponseDTO<PagedList<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceAsync(AmigoTenantTServiceSearchRequest search)
        {
            var queryFilter = GetQueryFilter(search);

            var report = await _serviceReportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            report.Items.ToList().ForEach(p =>
            {
                p.CostCenterCode = p.ChargeType == Constants.ChargeTypeCode.CostCenter ? p.ChargeNo : "";
                p.ShipmentID = p.ChargeType == Constants.ChargeTypeCode.Shipment ? p.ChargeNo : "";
            });

            var pagedResult = new PagedList<AmigoTenantTServiceReportDTO>()
            {
                Items = report.Items,
                PageSize = report.PageSize,
                Page = report.Page,
                Total = report.Total
            };

            return ResponseBuilder.Correct(pagedResult);

        }

        private Expression<Func<AmigoTenantTServiceReportDTO, bool>> GetQueryFilter(AmigoTenantTServiceSearchRequest search)
        {
            Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter = p => true;

            if (search.From.HasValue && search.To.HasValue)
            {
                var toPlusADay = search.To.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.From);
                queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);
            }
            else if (search.From.HasValue && !search.To.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ServiceStartDate.Value >= search.From);
            }
            else if (!search.From.HasValue && search.To.HasValue)
            {
                var toPlusADay = search.To.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ServiceStartDate.Value < toPlusADay);
            }

            if (search.DriverId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.DriverId);

            if (!string.IsNullOrEmpty(search.ChargeNoType))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.ChargeNoType));
            }
            if (!string.IsNullOrEmpty(search.EquipmentSizeCode))
                queryFilter = queryFilter.And(p => p.EquipmentSizeCode.Contains(search.EquipmentSizeCode));

            if (search.EquipmentTypeId > 0)
                queryFilter = queryFilter.And(p => p.EquipmentTypeId == search.EquipmentTypeId);

            return queryFilter;
        }

        public async Task<ResponseDTO<AmigoTenantTServiceReportDTO>> SearchByIdAsync(int amigoTenantTServiceId)
        {
            Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter = p => true;

            if (amigoTenantTServiceId > 0)
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantTServiceId == amigoTenantTServiceId.ToString());
            }

            var report = await _serviceReportDataAccess.FirstOrDefaultAsync(queryFilter);

            report.CostCenterCode = report.ChargeType == Constants.ChargeTypeCode.CostCenter ? report.ChargeNo : "";
            report.ShipmentID = report.ChargeType == Constants.ChargeTypeCode.Shipment ? report.ChargeNo : "";

            return ResponseBuilder.Correct(report);

        }

        public async Task<ResponseDTO> UpdateAmigoTenantTServiceForApproveAsync(AmigoTenantTServiceRequest maintenance)
        {
            Expression<Func<AmigoTenanttServiceDTO, bool>> queryFilter = p => true;
            queryFilter = queryFilter.And(p => p.AmigoTenantTServiceId == maintenance.AmigoTenantTServiceId);

            var amigoTenantTService = await _amigoTenantTServiceDataAccess.FirstOrDefaultAsync(queryFilter);
            var startDate = amigoTenantTService?.ServiceStartDate;
            if (startDate != null && maintenance.ServiceStartDate.HasValue)
            {
                startDate = startDate.Value.AddHours(maintenance.ServiceStartDate.Value.Hour - startDate.Value.Hour);
                startDate = startDate.Value.AddMinutes(maintenance.ServiceStartDate.Value.Minute - startDate.Value.Minute);
                startDate = startDate.Value.AddSeconds(maintenance.ServiceStartDate.Value.Second - startDate.Value.Second);
                maintenance.ServiceStartDate = startDate;
            }

            var finishDate = amigoTenantTService?.ServiceFinishDate;
            if (finishDate != null && maintenance.ServiceFinishDate.HasValue)
            {
                finishDate = finishDate.Value.AddHours(maintenance.ServiceFinishDate.Value.Hour - finishDate.Value.Hour);
                finishDate = finishDate.Value.AddMinutes(maintenance.ServiceFinishDate.Value.Minute - finishDate.Value.Minute);
                finishDate = finishDate.Value.AddSeconds(maintenance.ServiceFinishDate.Value.Second - finishDate.Value.Second);
                maintenance.ServiceFinishDate = finishDate;
            }
            else
            {
                maintenance.ServiceFinishDate = new DateTimeOffset(maintenance.ServiceFinishDate.Value.Year,
                    maintenance.ServiceFinishDate.Value.Month,
                    maintenance.ServiceFinishDate.Value.Day, maintenance.ServiceFinishDate.Value.Hour,
                    maintenance.ServiceFinishDate.Value.Minute,
                    maintenance.ServiceFinishDate.Value.Second, maintenance.ServiceStartDate.Value.Offset);
            }

            if (maintenance.ServiceStartDate.HasValue)
            {
                maintenance.ServiceStartDateLocal = maintenance.ServiceStartDate.Value.DateTime;

            }
            maintenance.ServiceStartDateUTC = DateTimeUTCCommon.DatetimeToDateUTC(maintenance.ServiceStartDate);
            if (maintenance.ServiceFinishDate.HasValue)
                maintenance.ServiceFinishDateUTC = DateTimeUTCCommon.DatetimeToDateUTC(maintenance.ServiceFinishDate);

            var command = _mapper.Map<AmigoTenantTServiceRequest, UpdateAmigoTenantTServiceApproveCommand>(maintenance);
            var activityType = await _activityTypeService.SearchActivityByCodeAsync(Constants.ActivityTypeCode.EditbeforeApproval);
            command.ActivityTypeId = activityType.ActivityTypeId;
            command.ChargeNo = maintenance.ChargeType == Constants.ChargeTypeCode.Shipment ? maintenance.ShipmentID : maintenance.CostCenterCode;
            var resp = await _bus.SendAsync(command);
            return resp.ToResponse().WithMessages(new ApplicationMessage() { Key = "", Message = Constants.SuccessMessages.Common.SaveSuccessfull });
        }

        public async Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceForApproveAsync(AmigoTenantTServiceApproveSearchRequest search)
        {
            try
            {
                List<OrderExpression<AmigoTenantTServiceReportDTO>> orderExpressionList = new List<OrderExpression<AmigoTenantTServiceReportDTO>>();
                orderExpressionList.Add(new OrderExpression<AmigoTenantTServiceReportDTO>(OrderType.Desc, p => p.ServiceStartDateLocal));

                Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter = p => true;

                if (!string.IsNullOrEmpty(search.PaidBy))
                {
                    queryFilter = queryFilter.And(p => p.PayBy == search.PaidBy);

                    if (search.PaidBy == Constants.PaidBy.PerHour)
                    {
                        if (search.ServiceDate.HasValue)
                        {
                            var toPlusADay = search.ServiceDate.Value.Date.AddDays(1);
                            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.ServiceDate.Value.Date);
                            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);
                        }
                    }
                    else
                    {
                        if (search.ReportDateFrom.HasValue && search.ReportDateTo.HasValue)
                        {
                            //queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.ReportDateFrom.Value.Date);
                            //queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value <= search.ReportDateTo.Value.Date);

                            var toPlusADay = search.ReportDateTo.Value.AddDays(1);
                            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= search.ReportDateFrom);
                            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);

                        }
                    }
                }

                if (search.DriverId > 0)
                    queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.DriverId);

                Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter1 = queryFilter;

                if (search.ServiceStatusId != -1)
                {
                    if (search.ServiceStatusId == 0)
                        queryFilter = queryFilter.And(p => p.ServiceStatus == false);
                    else if (search.ServiceStatusId == 1)
                        queryFilter = queryFilter.And(p => p.ServiceStatus == true);
                    else if (search.ServiceStatusId == 2)
                        queryFilter = queryFilter.And(p => p.ServiceStatus == null);
                }
                var report = await _serviceReportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

                Expression<Func<AmigoTenantTServiceReportDTO, bool>> countFilter = c => true;
                var approved = await _serviceReportDataAccess.CountAsync(queryFilter1.And(p => p.ServiceStatus == true));
                var rejected = await _serviceReportDataAccess.CountAsync(queryFilter1.And(p => p.ServiceStatus == false));
                var pending = await _serviceReportDataAccess.CountAsync(queryFilter1.And(p => p.ServiceStatus == null));
                var totals = await _serviceReportDataAccess.CountAsync(queryFilter1);

                var totalApproved = approved;
                var totalRejected = rejected;
                var totalPending = pending;
                var total = totals;

                var pagedResult = new PagedListServices<AmigoTenantTServiceReportDTO>()
                {
                    Items = report.Items,
                    PageSize = search.PageSize,
                    Page = search.Page,
                    Total = Convert.ToInt32(report.Total),
                    TotalPending = Convert.ToInt32(totalPending),
                    TotalApproved = Convert.ToInt32(totalApproved),
                    TotalRejected = Convert.ToInt32(totalRejected)
                };

                //var report = await _serviceReportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

                //var pagedResult = new PagedListServices<AmigoTenantTServiceReportDTO>()
                //{
                //    Items = report.Items,
                //    PageSize = report.PageSize,
                //    Page = report.Page,
                //    Total = report.Total,
                //    TotalPending = 20,
                //    TotalApproved = 15,
                //    TotalRejected = 40
                //};

                return ResponseBuilder.Correct(pagedResult);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceByChargeNumberAsync(AmigoTenantTServiceSearchChargeNumRequest search, int? driverId, int amigoTenantTServiceDateRangeDays)
        {
            List<OrderExpression<AmigoTenantTServiceReportDTO>> orderExpressionList =
                new List<OrderExpression<AmigoTenantTServiceReportDTO>>
                {
                   new OrderExpression<AmigoTenantTServiceReportDTO>(OrderType.Desc, p => p.ServiceStartDate)
                };

            Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter = p => true;

            var startDateFilter = DateTime.Today.AddDays(-amigoTenantTServiceDateRangeDays);
            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= startDateFilter);
            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < DateTime.Today.AddDays(1));

            if (driverId.HasValue && driverId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == driverId);


            if (!string.IsNullOrEmpty(search.ChargeNumber))
            {
                queryFilter =
                    queryFilter.And(
                        p =>    p.ChargeNo.Contains(search.ChargeNumber) || 
                                p.EquipmentNumber.Contains(search.ChargeNumber) || 
                                p.ChassisNumber.Contains(search.ChargeNumber) 
                        );
            }
            var report = await _serviceReportDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize,
                        orderExpressionList.ToArray());

            report.Items.ToList().ForEach(p =>
            {
                p.CostCenterCode = p.ChargeType == Constants.ChargeTypeCode.CostCenter ? p.ChargeNo : "";
                p.ShipmentID = p.ChargeType == Constants.ChargeTypeCode.Shipment ? p.ChargeNo : "";
            });

            var totals = await _serviceReportDataAccess.CountAsync(queryFilter);

            var total = totals;

            var pagedResult = new PagedListServices<AmigoTenantTServiceReportDTO>()
            {
                Items = report.Items,
                PageSize = search.PageSize,
                Page = search.Page,
                Total = Convert.ToInt32(total)
            };


            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO> ApproveAmigoTenantTServiceAsync(AmigoTenantTServiceApproveRequest request)
        {
            try
            {
                ResponseDTO res = new ResponseDTO();

                var driverIds = request.AmigoTenantTServiceIdsListStatus.Select(q => q.DriverId).Distinct();
                var fromDate = request.MoveOrHour == Constants.PaidBy.PerHour ? request.ReportDate.Value.Date : request.FromDate.Value.Date;
                var toDate = request.MoveOrHour == Constants.PaidBy.PerHour ? request.ReportDate.Value.Date : request.ToDate.Value.Date;

                foreach (var driverId in driverIds)
                {
                    var nextDate = fromDate;
                    RegisterDriverReportCommand driverReportCommand = new RegisterDriverReportCommand(request.DriverId);

                    request.AmigoTenantTServiceIdsListStatus.Where(q => q.DriverId == driverId).ToList().ForEach(item =>
                    {
                        driverReportCommand.AmigoTenantTServiceIdsListStatus.Add(new Commands.Tracking.Approve.AmigoTenantTServiceStatus()
                        {
                            AmigoTenantTServiceId = item.AmigoTenantTServiceId,
                            ServiceStatus = item.ServiceStatus,
                            DriverId = item.DriverId
                        });

                    });

                    while (nextDate <= toDate)
                    {
                        //GETTING APPROVE RATES
                        Expression<Func<AmigoTenantTServiceApproveRateDTO, bool>> queryFilter = p => driverReportCommand.AmigoTenantTServiceIdsListStatus.Select(x => x.AmigoTenantTServiceId).Contains(p.AmigoTenantTServiceId);
                        var toPlusADay = nextDate.Date.AddDays(1);
                        queryFilter = queryFilter.And(p => p.ServiceStartDate.Value >= nextDate.Date);
                        queryFilter = queryFilter.And(p => p.ServiceStartDate.Value < toPlusADay);
                        var approveRates = await _serviceApproveRateDataAccess.ListAsync(queryFilter);

                        if (approveRates.Any())
                        {
                            await PrepareDriverReportAndChargesCommand(request, approveRates.ToList(), driverReportCommand, driverId, nextDate);

                            if (driverReportCommand.AmigoTenantTServiceCharges != null && driverReportCommand.AmigoTenantTServiceCharges.Any())
                            {
                                await PrepareAmigoTenantTServicesForApproveOrRejectStatus(request, driverReportCommand.AmigoTenantTServiceCharges);
                                var resp = await _bus.SendAsync(driverReportCommand);
                                res = resp.ToResponse().WithMessages(new ApplicationMessage() { Key = "", Message = Constants.SuccessMessages.ApproveRejectProcess.ApproveRejectSuccessfully });
                            }
                        }

                        nextDate = nextDate.Date.AddDays(1);

                    }

                }

                return res;
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsValid = false, Messages = new List<ApplicationMessage>() { new ApplicationMessage { Key = "", Message = ex.Message } } };
            }

        }

        private async Task PrepareDriverReportAndChargesCommand(AmigoTenantTServiceApproveRequest request, List<AmigoTenantTServiceApproveRateDTO> approveRates, RegisterDriverReportCommand driverReportCommand, int? driverId, DateTime? date)
        {
            //if (approveRates.Any())
            //{
            var hasPerMoveSvc = approveRates.Any(q => q.PayBy == Constants.PaidBy.PerMove);
            var hasPerHourSvc = approveRates.Any(q => q.PayBy == Constants.PaidBy.PerHour);
            List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges = new List<RegisterAmigoTenantTServiceChargeCommand>();

            if (hasPerMoveSvc)
            {
                var servicesRateOnlyPerMove = approveRates.Where(q => q.PayBy == Constants.PaidBy.PerMove).ToList();
                await PerMoveCalculate(request.UserId, amigoTenantTServiceCharges, servicesRateOnlyPerMove, request.IsApprove);
            }

            if (hasPerHourSvc)
                await PerHourCalculate(request, amigoTenantTServiceCharges, driverReportCommand, driverId, request.IsApprove, date);


            await SetDriverReportCommandFromCharges(request, driverReportCommand, amigoTenantTServiceCharges, driverId, date);
            //}
        }

        private async Task SetDriverReportCommandFromCharges(AmigoTenantTServiceApproveRequest request, RegisterDriverReportCommand driverReportCommand, List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges, int? driverId, DateTime? date)
        {
            decimal? driverPay = 0;
            decimal? customerBill = 0;
            decimal? totalHours = 0;

            foreach (var charge in amigoTenantTServiceCharges)
            {
                driverPay += charge.DriverPay;
                customerBill += charge.CustomerBill;
                totalHours += charge.TotalHours;
            }

            //TODO: PASS DATE AS PARAMETER AS FILTER
            //TODO: PASS DATE AS PARAMETER AS FILTER
            //TODO: PASS DATE AS PARAMETER AS FILTER
            driverReportCommand.ReportDate = date.Value.Date; ////TODO: PASS DATE AS PARAMETER AS FILTER
            driverReportCommand.Year = date.Value.Year;
            driverReportCommand.WeekNumber = (int?)date.Value.DayOfWeek;
            driverReportCommand.DriverUserId = driverId;
            driverReportCommand.ApproverUserId = request.UserId;
            driverReportCommand.ApproverSignature = request.ApprovedBy;
            driverReportCommand.Username = request.DriverName;
            driverReportCommand.DayPayDriverTotal = driverPay;
            driverReportCommand.DayBillCustomerTotal = customerBill;
            driverReportCommand.TotalHours = totalHours;
            driverReportCommand.RowStatus = true;
            driverReportCommand.AmigoTenantTServiceCharges = amigoTenantTServiceCharges;

            var activityType = await _activityTypeService.SearchActivityByCodeAsync(Constants.ActivityTypeCode.ApproveDriverReport);
            if (activityType != null)
                driverReportCommand.ActivityTypeId = activityType.ActivityTypeId;
        }

        private async Task PerMoveCalculate(int? sessionUserId, List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges, List<AmigoTenantTServiceApproveRateDTO> approveRates, bool? approveOrReject)
        {

            if (approveRates.Any())
            {
                RegisterAmigoTenantTServiceChargeCommand amigoTenantTServiceCharge;
                decimal? dayPayDriverCalutated = 0;
                decimal? dayBillCustomerCalculated = 0;

                foreach (var approveRate in approveRates)
                {
                    amigoTenantTServiceCharge = new RegisterAmigoTenantTServiceChargeCommand();
                    amigoTenantTServiceCharge.AmigoTenantTServiceId = int.Parse(approveRate.AmigoTenantTServiceId);
                    amigoTenantTServiceCharge.RowStatus = true;
                    amigoTenantTServiceCharge.CreatedBy = sessionUserId;
                    amigoTenantTServiceCharge.CreationDate = DateTime.UtcNow;
                    amigoTenantTServiceCharge.ApproveOrReject = approveOrReject;

                    if (approveOrReject.Value)
                    {
                        amigoTenantTServiceCharge.RateId = approveRate.RateId;
                        decimal? totalHoursRounded = 0;
                        if (approveRate.ServiceCode == Constants.ServiceCode.DET ||
                            approveRate.ServiceCode == Constants.ServiceCode.LFT)
                        {
                            //CALCULATE BY HOURS
                            totalHoursRounded = RoundHour(approveRate.TotalHours);
                            dayPayDriverCalutated = approveRate.PayDriver.Value * totalHoursRounded;
                            dayBillCustomerCalculated = approveRate.BillCustomer.Value * totalHoursRounded;
                        }
                        else
                        {
                            //CALCULATE BY MOVE
                            dayPayDriverCalutated = approveRate.PayDriver.Value;
                            dayBillCustomerCalculated = approveRate.BillCustomer.Value;
                        }

                        amigoTenantTServiceCharge.DriverPay = dayPayDriverCalutated;
                        amigoTenantTServiceCharge.CustomerBill = dayBillCustomerCalculated;
                        amigoTenantTServiceCharge.TotalHours = totalHoursRounded;
                        amigoTenantTServiceCharge.PayBy = Constants.PaidBy.PerMove;

                    }
                    amigoTenantTServiceCharges.Add(amigoTenantTServiceCharge);
                }
            }
        }

        private decimal? RoundHour(decimal? value)
        {
            decimal? smallerInt = decimal.Floor(value.Value);
            decimal? decimalPart = value - smallerInt;
            decimal? results = value;

            if (decimalPart > 0 && decimalPart.Value.CompareTo((decimal)0.25) < 0)
                results = (decimal?)0.25;
            else if (decimalPart.Value.CompareTo((decimal)0.25) > 0 && decimalPart.Value.CompareTo((decimal)0.5) < 0)
                results = (decimal?)0.50;
            else if (decimalPart.Value.CompareTo((decimal)0.5) > 0 && decimalPart.Value.CompareTo((decimal)0.75) < 0)
                results = (decimal?)0.75;
            else if (decimalPart.Value.CompareTo((decimal)0.75) > 0 && decimalPart.Value.CompareTo((decimal)1) <= 0)
                results = 1;
            else
                results = decimalPart;

            return smallerInt + results;


        }

        private async Task PerHourCalculate(AmigoTenantTServiceApproveRequest request, List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges, RegisterDriverReportCommand driverReportCommand, int? driverId, bool? approveOrReject, DateTime? date)
        {
            #region Selecting EventLogs Activities

            //SELECTING EVENTLOGS FOR A WHOLE DAY AND DRIVER

            //OrderExpression
            List<OrderExpression<AmigoTenantTEventLogPerHourDTO>> orderExpressionList = new List<OrderExpression<AmigoTenantTEventLogPerHourDTO>>();
            orderExpressionList.Add(new OrderExpression<AmigoTenantTEventLogPerHourDTO>(OrderType.Asc, p => p.ReportedActivityDateLocal));
            //QueryExpression
            Expression<Func<AmigoTenantTEventLogPerHourDTO, bool>> queryFilterPerHour = p => true;
            queryFilterPerHour = queryFilterPerHour.And(p => p.AmigoTenantTUserId == driverId);
            var toPlusADay = date.Value.Date.AddDays(1); // request.ReportDate.Value.Date.AddDays(1);
            queryFilterPerHour = queryFilterPerHour.And(p => p.ReportedActivityDateLocal.Value >= date.Value.Date);
            queryFilterPerHour = queryFilterPerHour.And(p => p.ReportedActivityDateLocal.Value < toPlusADay);

            var approveRatesPerHour = await _amigoTenantTEventLogPerHourDataAccess.ListAsync(queryFilterPerHour, orderExpressionList.ToArray());

            #endregion

            #region Calculo
            //CALCULATE DESTINATION - ORIGIN - SUM(ONDUTY - BREAK)
            var firstStartWorkDay = approveRatesPerHour.FirstOrDefault(q => q.ActivityTypeCode == Constants.ActivityTypeCode.StartWorkday);
            var lastFinishWorkDay = approveRatesPerHour.LastOrDefault(q => q.ActivityTypeCode == Constants.ActivityTypeCode.FinishWorkday);
            var firstOnDuty = approveRatesPerHour.FirstOrDefault(q => q.ActivityTypeCode == Constants.ActivityTypeCode.OnDuty);

            decimal? totalTimeStartToFinishACtivity = 0;
            decimal? totalHoursPerHour = 0;
            decimal? dayPayDriverCalutated = 0;
            decimal? dayBillCustomerCalculated = 0;

            if (firstStartWorkDay == null || lastFinishWorkDay == null)
                throw new Exception(Constants.ErrorMessages.ApproveRejectProcess.MissingStartOrFinishWorkDay);

            var elementsToCalculate = approveRatesPerHour.Where(q =>
                                                                (q.ActivityTypeCode == Constants.ActivityTypeCode.OnBreak ||
                                                                q.ActivityTypeCode == Constants.ActivityTypeCode.OnDuty)
                                                                && q.AmigoTenantTEventLogId != firstOnDuty.AmigoTenantTEventLogId
                                                                );

            int index = 0;
            decimal? totalOnBreaks = 0;
            foreach (var currentElement in elementsToCalculate)
            {
                if (index + 1 < elementsToCalculate.Count())
                {
                    var nextElement = elementsToCalculate.ToArray()[index + 1];
                    if (currentElement.ActivityTypeCode == Constants.ActivityTypeCode.OnBreak &&
                        nextElement.ActivityTypeCode == Constants.ActivityTypeCode.OnDuty)
                    {
                        totalOnBreaks += (decimal?)nextElement.ReportedActivityDateLocal.Value.Subtract(currentElement.ReportedActivityDateLocal.Value).TotalHours;
                    }
                    index++;
                }
            }

            totalTimeStartToFinishACtivity = (decimal?)lastFinishWorkDay.ReportedActivityDateLocal.Value.Subtract(firstStartWorkDay.ReportedActivityDateLocal.Value).TotalHours;
            totalHoursPerHour = totalTimeStartToFinishACtivity - totalOnBreaks;

            #endregion

            #region Creation of Charges

            Expression<Func<RateDTO, bool>> queryFilterRate = p => true;
            queryFilterRate = queryFilterRate.And(p => p.Code == Constants.RateCode.Hourly);
            var rate = await _rateDataAccess.FirstOrDefaultAsync(queryFilterRate);


            if (rate == null)
                throw new Exception(Constants.ErrorMessages.ApproveRejectProcess.MissingRate);

            //ASSIGNING CHARGES FROM SERVICES BY HOUR

            //TODO: PASS DATE AS PARAMETER AS FILTER
            //TODO: PASS DATE AS PARAMETER AS FILTER
            //TODO: PASS DATE AS PARAMETER AS FILTER
            var listServicesByHour = await GetServicePayedByHours(request, driverId, date);
            RegisterAmigoTenantTServiceChargeCommand amigoTenantTServiceCharge = null;

            foreach (var item in listServicesByHour)
            {
                amigoTenantTServiceCharge = new RegisterAmigoTenantTServiceChargeCommand();
                amigoTenantTServiceCharge.AmigoTenantTServiceId = int.Parse(item.AmigoTenantTServiceId);
                amigoTenantTServiceCharge.RowStatus = true;
                amigoTenantTServiceCharge.CreatedBy = request.UserId;
                amigoTenantTServiceCharge.CreationDate = DateTime.UtcNow;
                amigoTenantTServiceCharge.RateId = rate.RateId;
                amigoTenantTServiceCharge.DriverPay = 0;
                amigoTenantTServiceCharge.CustomerBill = 0;
                amigoTenantTServiceCharge.TotalHours = 0;
                amigoTenantTServiceCharge.PayBy = Constants.PaidBy.PerHour;
                amigoTenantTServiceCharge.ApproveOrReject = approveOrReject;
                amigoTenantTServiceCharges.Add(amigoTenantTServiceCharge);
            }




            //ASSIGNING CHARGE HOURLY
            amigoTenantTServiceCharge = new RegisterAmigoTenantTServiceChargeCommand();
            dayPayDriverCalutated = rate.PayDriver.Value * (decimal)totalHoursPerHour;
            dayBillCustomerCalculated = rate.BillCustomer.Value * (decimal)totalHoursPerHour;

            amigoTenantTServiceCharge.AmigoTenantTServiceId = null;
            amigoTenantTServiceCharge.RowStatus = true;
            amigoTenantTServiceCharge.CreatedBy = request.UserId;
            amigoTenantTServiceCharge.CreationDate = DateTime.UtcNow;
            amigoTenantTServiceCharge.RateId = rate.RateId;
            amigoTenantTServiceCharge.DriverPay = dayPayDriverCalutated;
            amigoTenantTServiceCharge.CustomerBill = dayBillCustomerCalculated;
            amigoTenantTServiceCharge.TotalHours = totalHoursPerHour;
            amigoTenantTServiceCharge.PayBy = Constants.PaidBy.PerHour;
            amigoTenantTServiceCharge.ApproveOrReject = approveOrReject;
            amigoTenantTServiceCharges.Add(amigoTenantTServiceCharge);


            #endregion

            driverReportCommand.StartTime = firstStartWorkDay.ReportedActivityDateLocal;
            driverReportCommand.FinishTime = lastFinishWorkDay.ReportedActivityDateLocal;
        }

        private async Task PrepareAmigoTenantTServicesForApproveOrRejectStatus(AmigoTenantTServiceApproveRequest request, List<RegisterAmigoTenantTServiceChargeCommand> amigoTenantTServiceCharges)
        {
            if (amigoTenantTServiceCharges != null && amigoTenantTServiceCharges.Any())
            {

                foreach (var amigoTenantTServiceCharge in amigoTenantTServiceCharges)
                {

                    if (amigoTenantTServiceCharge.AmigoTenantTServiceId.HasValue)
                    {
                        UpdateAmigoTenantServiceCommand command = new UpdateAmigoTenantServiceCommand();
                        command.AmigoTenantTServiceId = amigoTenantTServiceCharge.AmigoTenantTServiceId.Value;
                        command.ApprovalModified = Constants.RowStatusString.Active;
                        command.UpdatedBy = (int)request.UserId;
                        command.UpdatedDate = DateTime.UtcNow;
                        command.ServiceStatus = amigoTenantTServiceCharge.ApproveOrReject;
                        command.ApprovalComments = request.ApprovalComments;

                        if (command.ServiceStatus.Value)
                        {
                            command.ApprovedBy = request.ApprovedBy;
                            command.ApprovalDate = DateTime.UtcNow;
                        }

                        amigoTenantTServiceCharge.AmigoTenantTService = command;
                    }
                }

            }
        }

        private async Task<List<AmigoTenantTServiceReportDTO>> GetServicePayedByHours(AmigoTenantTServiceApproveRequest request, int? driverId, DateTime? date)
        {
            Expression<Func<AmigoTenantTServiceReportDTO, bool>> queryFilter = p => true;

            var toPlusADay = date.Value.Date.AddDays(1);  //request.ReportDate.Value.Date.AddDays(1);
            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value >= date.Value.Date);// request.ReportDate.Value.Date);
            queryFilter = queryFilter.And(p => p.ServiceStartDateLocal.Value < toPlusADay);
            queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == driverId);
            queryFilter = queryFilter.And(p => p.ServiceStatus == null);
            queryFilter = queryFilter.And(p => p.PayBy == Constants.PaidBy.PerHour);
            var servicesByHour = await _serviceReportDataAccess.ListAsync(queryFilter);
            return servicesByHour.ToList();

        }

        //AKNOWLEDGE
        //AKNOWLEDGE
        //AKNOWLEDGE
        public async Task<ResponseDTO> UpdateAmigoTenantTServiceAckAsync(UpdateAmigoTenantTServiceAckRequest maintenance, string userName, int userId)
        {
            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(maintenance.ServiceAcknowledgeDateTZ);
            if (!string.IsNullOrEmpty(tz.Id))
            {
                maintenance.ServiceAcknowledgeDateTZ = tz.Id;
            }

            foreach (var p in maintenance.ServiceOrderNoList)
            {
                if (!p.AmigoTenantTServiceId.HasValue || p.AmigoTenantTServiceId.Value == 0)
                {
                    var amigoTenantService = await VerifyServiceOrderNoExists(userId, p.ServiceOrderNo);
                    if (amigoTenantService != null)
                        p.AmigoTenantTServiceId = amigoTenantService.AmigoTenantTServiceId;

                }
            }

            var command = _mapper.Map<UpdateAmigoTenantTServiceAckRequest, UpdateAmigoTenantServiceAckCommand>(maintenance);
            command.ServiceAcknowledgeDateUTC = DateTimeUTCCommon.DatetimeToDateUTC(command.ServiceAcknowledgeDate);
            command.IsAknowledged = true;
            command.Username = userName;
            command.AmigoTenantTServiceIdList = new List<int?>();
            command.AmigoTenantTServiceIdList.AddRange(maintenance.ServiceOrderNoList.Select(p => p.AmigoTenantTServiceId));

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return resp.ToResponse();
        }

        #region Export to excel
        public async void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, AmigoTenantTServiceSearchRequest search)
        {
            var list = await SearchAmigoTenantTServiceAsync(search);
            try
            {
                if (list.Data.Items.Count > 0)
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string> {
                            "Charge Type","Charge No","Equipment No","Type","Chassis No","Service","From Block","To Block","Start","Finish","Product","Approved By","Dispatching","Driver"
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

        private string ProcessCellDataToReport(AmigoTenantTServiceReportDTO item)
        {
            var startDate = string.Format("{0:MM/dd/yyyy}", item.ServiceStartDateLocal) ?? "";
            var finishDate = string.Format("{0:MM/dd/yyyy}", item.ServiceFinishDate) ?? "";
            var chargeType = item.ChargeType == "C" ? "Cost Center" : "ShipmentID";
            var product = !string.IsNullOrEmpty(item.ProductName) ? item.ProductName.Replace(@",", ".") : "";
            string textProperties = ExcelHelper.StringToCSVCell(chargeType) + "," + ExcelHelper.StringToCSVCell(item.ChargeNo) + "," + ExcelHelper.StringToCSVCell(item.EquipmentNumber) + "," + ExcelHelper.StringToCSVCell(item.EquipmentTypeCode) + "," +
                                    ExcelHelper.StringToCSVCell(item.ChassisNumber) + "," + ExcelHelper.StringToCSVCell(item.ServiceName) + "," + ExcelHelper.StringToCSVCell(item.OriginLocationName) + "," + ExcelHelper.StringToCSVCell(item.DestinationLocationCode) + "," +
                                    ExcelHelper.StringToCSVCell(startDate) + "," +
                                    ExcelHelper.StringToCSVCell(finishDate) + "," +
                                    ExcelHelper.StringToCSVCell(product) + "," + ExcelHelper.StringToCSVCell(item.ApprovedBy) + "," + ExcelHelper.StringToCSVCell(item.DispatchingPartyCode) + "," + ExcelHelper.StringToCSVCell(item.UserName);

            return textProperties;
        }
        #endregion

    }


}

