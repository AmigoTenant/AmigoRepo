using System;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.Services.Common;
using System.Linq.Expressions;
using Amigo.Tenant.Application.Services.Common.Extensions;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;


namespace Amigo.Tenant.Application.Services.Tracking
{
   public class AmigoTenantTEventLogApplicationService : IAmigoTenantTEventLogApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AmigoTenantTEventLogDTO> _logDataAccess;
        private readonly IQueryDataAccess<AmigoTenantTEventLogSearchResultDTO> _logSearchDataAccess;
        private readonly IActivityTypeApplicationService _activityApplicationService;

        public AmigoTenantTEventLogApplicationService(IBus bus,
            IQueryDataAccess<AmigoTenantTEventLogDTO> logDataAccess,
            IQueryDataAccess<AmigoTenantTEventLogSearchResultDTO> logSearchDataAccess,
            IActivityTypeApplicationService activityApplicationService,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _logDataAccess = logDataAccess;
            _logSearchDataAccess = logSearchDataAccess;
            _activityApplicationService = activityApplicationService;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> RegisterAmigoTenantTEventLogAsync(AmigoTenantTEventLogDTO maintenance)
        {

            var model = await _activityApplicationService.SearchActivityByCodeAsync(maintenance.ActivityCode);
            if (model != null)
            {
                maintenance.ActivityTypeId = model.ActivityTypeId;
            }

            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(maintenance.ReportedActivityTimeZone);
            if (!string.IsNullOrEmpty(tz.Id))
            {
                maintenance.ReportedActivityTimeZone = tz.Id;
            }
            maintenance.ConvertedActivityUTC = DateTimeUTCCommon.DatetimeToDateUTC(maintenance.ReportedActivityDate);

            var command = _mapper.Map<AmigoTenantTEventLogDTO, RegisterAmigoTenanttEventLogCommand>(maintenance);

            //Execute Command

            var resp = await _bus.SendAsync(command);
            
            return resp.ToResponse();
        }

        
        public async Task<ResponseDTO<PagedList<AmigoTenantTEventLogSearchResultDTO>>> SearchAsync(AmigoTenantTEventLogSearchRequest search)
        {

            var queryFilter = GetQueryFilter(search);

            var eventLogs = await _logSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<AmigoTenantTEventLogSearchResultDTO>()
            {

                Items = eventLogs.Items,
                PageSize = eventLogs.PageSize,
                Page = eventLogs.Page,
                Total = eventLogs.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }


        #region Helpers

        private Expression<Func<AmigoTenantTEventLogSearchResultDTO, bool>> GetQueryFilter(AmigoTenantTEventLogSearchRequest search)
        {
            Expression<Func<AmigoTenantTEventLogSearchResultDTO, bool>> queryFilter = p => true;
            

            if (!string.IsNullOrEmpty(search.ActivityTypeCode))
            {
                queryFilter = queryFilter.And(p => p.ActivityTypeCode.Contains(search.ActivityTypeCode));
            }

            if (!string.IsNullOrEmpty(search.Username))
            {
                queryFilter = queryFilter.And(p => p.Username.Contains(search.Username));
            }

            if (search.ReportedActivityDateFrom != null)
            {
                queryFilter = queryFilter.And(p => p.ReportedActivityDate >= search.ReportedActivityDateFrom);
            }

            if (search.ReportedActivityDateTo != null)
            {
                queryFilter = queryFilter.And(p => p.ReportedActivityDate <= search.ReportedActivityDateTo);
            }

            if (!string.IsNullOrEmpty(search.ReportedActivityTimeZone))
            {
                queryFilter = queryFilter.And(p => p.ReportedActivityTimeZone.Contains(search.ReportedActivityTimeZone));
            }

            if (!string.IsNullOrEmpty(search.LogType))
            {
                queryFilter = queryFilter.And(p => p.LogType.Contains(search.LogType));
            }

            if (!string.IsNullOrEmpty(search.Parameters))
            {
                queryFilter = queryFilter.And(p => p.Parameters.Contains(search.Parameters));
            }


            if (!string.IsNullOrEmpty(search.AmigoTenantMoveNumber))
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantMoveNumber == search.AmigoTenantMoveNumber);
            }

            if (search.AmigoTenantMoveId != null)
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantMoveId == search.AmigoTenantMoveId);
            }

            if (!string.IsNullOrEmpty(search.ShipmentID) && !string.IsNullOrEmpty(search.CostCenterCode))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo == search.ShipmentID || p.ChargeNo == search.CostCenterCode);
            }
            else
            {
                if (!string.IsNullOrEmpty(search.ShipmentID))
                {
                    queryFilter = queryFilter.And(p => p.ChargeNo == search.ShipmentID);
                }

                if (!string.IsNullOrEmpty(search.CostCenterCode))
                {
                    queryFilter = queryFilter.And(p => p.ChargeNo == search.CostCenterCode);
                }

            }

            //if (search.CostCenterId != null)
            //{
            //    queryFilter = queryFilter.And(p => p.CostCenterId == search.CostCenterId);
            //}

            if (!string.IsNullOrEmpty(search.EquipmentNumber))
            {
                queryFilter = queryFilter.And(p => p.EquipmentNumber == search.EquipmentNumber);
            }

            if (search.EquipmentId != null)
            {
                queryFilter = queryFilter.And(p => p.EquipmentId == search.EquipmentId);
            }

            if (search.IsAutoDateTime != null)
            {
                queryFilter = queryFilter.And(p => p.IsAutoDateTime == search.IsAutoDateTime);
            }

            if (search.IsSpoofingGPS != null)
            {
                queryFilter = queryFilter.And(p => p.IsSpoofingGPS == search.IsSpoofingGPS);
            }

            if (search.IsRootedJailbreaked != null)
            {
                queryFilter = queryFilter.And(p => p.IsRootedJailbreaked == search.IsRootedJailbreaked);
            }

            if (!string.IsNullOrEmpty(search.Platform))
            {
                queryFilter = queryFilter.And(p => p.Platform == search.Platform);
            }

            if (!string.IsNullOrEmpty(search.OSVersion))
            {
                queryFilter = queryFilter.And(p => p.OSVersion == search.OSVersion);
            }

            if (!string.IsNullOrEmpty(search.AppVersion))
            {
                queryFilter = queryFilter.And(p => p.AppVersion == search.AppVersion);
            }

            if (!string.IsNullOrEmpty(search.LocationProvider))
            {
                queryFilter = queryFilter.And(p => p.LocationProvider == search.LocationProvider);
            }
           

            return queryFilter;
        }


        #endregion


    }
}
