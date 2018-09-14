using System;
using System.Linq;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System.Linq.Expressions;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System.Collections.Generic;
using Newtonsoft.Json;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using System.Web;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class Last24HoursApplicationService : ILast24HoursApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<Last24HoursDTO> _last24HoursDataAccess;
        public ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }



        public Last24HoursApplicationService(IBus bus,
            IQueryDataAccess<Last24HoursDTO> last24HoursDataAccess,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _last24HoursDataAccess = last24HoursDataAccess;
            _mapper = mapper;
        }



        public async Task<ResponseDTO<List<Last24HoursDTO>>> SearchAsync(Last24HoursRequest search)
        {
            //---------------------------------------------------------------------------------------------
            //----------------  Get driver's last 24 hours logs from AmigoTenant DB -------------------
            //---------------------------------------------------------------------------------------------
            var queryFilter = GetQueryFilter(search);
            IEnumerable<Last24HoursDTO> eventLogs;

            Expression<Func<Last24HoursDTO, object>> expressionReportedActivityDate = p => p.ReportedActivityDate;
            List<OrderExpression<Last24HoursDTO>> orderExpressions = new List<OrderExpression<Last24HoursDTO>>();
            orderExpressions.Add(new OrderExpression<Last24HoursDTO>(OrderType.Desc, expressionReportedActivityDate));

            var mostRecent = await _last24HoursDataAccess.FirstOrDefaultAsync(queryFilter, orderExpressions.ToArray());

            if (mostRecent != null)
            {
                var before24Hours = mostRecent.ReportedActivityDate.Value.Subtract(new TimeSpan(24, 0, 0));
                eventLogs =
                    await
                        _last24HoursDataAccess.ListAsync(queryFilter.And(w => w.ReportedActivityDate >= before24Hours));
                //eventLogs = eventLogs.Select(s => s.);
                eventLogs.ToList().ForEach(x => x.Username = x.Username.ToUpper());

                var myGroup = eventLogs.GroupBy(
                    p => new
                    {
                        p.AmigoTenantTUserId,
                        p.Username,
                        p.Latitude,
                        p.Longitude,
                        p.ChargeNo,
                        p.ChargeType,
                        p.ActivityTypeName,
                        p.ActivityTypeCode,
                        p.TractorNumber,
                        p.FirstName,
                        p.LastName,
                        p.ChassisNumber,
                        p.EquipmentNumber
                    }

                    );
                eventLogs = myGroup
                    .Select(el => new Last24HoursDTO
                    {

                        AmigoTenantTUserId = el.Key.AmigoTenantTUserId,
                        Username = el.Key.Username,
                        Latitude = el.Key.Latitude,
                        Longitude = el.Key.Longitude,
                        ChargeNo = el.Key.ChargeNo,
                        ChargeType = el.Key.ChargeType,
                        ActivityTypeName = el.Key.ActivityTypeName,
                        ActivityTypeCode = el.Key.ActivityTypeCode,
                        TractorNumber = el.Key.TractorNumber,
                        FirstName = el.Key.FirstName,
                        LastName = el.Key.LastName,
                        ChassisNumber = el.Key.ChassisNumber,
                        EquipmentNumber = el.Key.EquipmentNumber,
                        ReportedActivityTimeZone = el.Last().ReportedActivityTimeZone,
                        ReportedActivityDate = el.Last().ReportedActivityDate,
                        Product = el.Last().Product,
                        Origin = el.Last().Origin,
                        Destination = el.Last().Destination,
                        ServiceName = el.Last().ServiceName,
                        EquipmentTypeName = el.Last().EquipmentTypeName
                    }).ToList().OrderByDescending(o => o.ReportedActivityDate);
            }
            else
            {
                List<Last24HoursDTO> res = new List<Last24HoursDTO>();
                return ResponseBuilder.Correct(res);
            }

            //---------------------------------------------------------------------------------------------
            //----------------  Get driver's firstName/lastName from Identity server ----------------------
            //---------------------------------------------------------------------------------------------


            string usernameParameter = "username=" + HttpUtility.UrlEncode(eventLogs.FirstOrDefault().Username);


            var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings);
            var rspUserDetails = await httpClient.GetAsync("api/Users/GetUser?" + usernameParameter);

            if (rspUserDetails.IsSuccessStatusCode)
            {
                var userDetailsJson = await rspUserDetails.Content.ReadAsStringAsync().ConfigureAwait(false);
                var user = JsonConvert.DeserializeObject<ResponseDTO<UserResponse>>(userDetailsJson);

                //----------------------------------------------------------------------
                //----------------------    merge with results list -------------------
                //----------------------------------------------------------------------
                int i = 0;
                foreach (var log in eventLogs)
                {
                    i++;
                    log.FirstName = user.Data.FirstName;
                    log.LastName = user.Data.LastName;
                    log.Index = i;
                }

                return ResponseBuilder.Correct(eventLogs.ToList());

            }
            else
            {
                throw new Exception("Amigo.Tenant.Application.Services.Tracking - Last24HoursService - SearchAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
            }
        }

        #region Helpers

        private Expression<Func<Last24HoursDTO, bool>> GetQueryFilter(Last24HoursRequest search)
        {
            Expression<Func<Last24HoursDTO, bool>> queryFilter = p => true;

            if (search.AmigoTenantTUserId != null)
            {
                queryFilter = queryFilter.And(p => p.AmigoTenantTUserId == search.AmigoTenantTUserId);
            }


            if (!string.IsNullOrEmpty(search.TractorNumber))
            {
                queryFilter = queryFilter.And(p => p.TractorNumber.Contains(search.TractorNumber));
            }

            if (!string.IsNullOrEmpty(search.ShipmentIdOrCostCenterCode))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.ShipmentIdOrCostCenterCode));
            }

            if (!string.IsNullOrEmpty(search.ChassisNumber))
            {
                queryFilter = queryFilter.And(p => p.ChassisNumber.Contains(search.ChassisNumber));
            }

            if (!string.IsNullOrEmpty(search.EquipmentNumber))
            {
                queryFilter = queryFilter.And(p => p.EquipmentNumber.Contains(search.EquipmentNumber));
            }

            //if (!string.IsNullOrEmpty(search.ActivityTypeCode))
            //{
            //    queryFilter = queryFilter.And(p => p.ActivityTypeCode == search.ActivityTypeCode);
            //}

            if (search.ActivityTypeCodes != null && search.ActivityTypeCodes.Count() > 0)
                queryFilter = queryFilter.And(p => search.ActivityTypeCodes.Contains(p.ActivityTypeCode));


            return queryFilter;
        }

        #endregion

    }
}
