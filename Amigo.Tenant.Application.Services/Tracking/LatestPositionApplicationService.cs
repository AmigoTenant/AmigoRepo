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

    public class LatestPositionApplicationService : ILatestPositionApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<LatestPositionDTO> _latestPositionDataAccess;
        public ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }

        public LatestPositionApplicationService(IBus bus,
            IQueryDataAccess<LatestPositionDTO> latestPositionDataAccess,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _latestPositionDataAccess = latestPositionDataAccess;
            _mapper = mapper;
        }


        public async Task<ResponseDTO<List<LatestPositionDTO>>> SearchAsync(LatestPositionRequest search)
        {
            //---------------------------------------------------------------------------------------------
            //----------------  Get driver's latest position from AmigoTenant DB ----------------------
            //---------------------------------------------------------------------------------------------

            var queryFilter = GetQueryFilter(search);
            var eventLogs = await _latestPositionDataAccess.ListAsync(queryFilter);
            eventLogs = eventLogs.OrderByDescending(x => x.ReportedActivityDate); //TODO FAVIO => use order expression in the previous line
            foreach (LatestPositionDTO element in eventLogs)
            {
                if (!string.IsNullOrEmpty(element.Username))
                    element.Username = element.Username.ToUpper();
            }
            var latestPositions = eventLogs.GroupBy(
                                                        p => new
                                                        {
                                                            p.Username
                                                        }

                                    ).Select(el => new LatestPositionDTO
                                    {
                                        AmigoTenantTEventLogId = el.First().AmigoTenantTEventLogId,
                                        Username = el.Key.Username,
                                        ReportedActivityDate = el.First().ReportedActivityDate,
                                        ReportedActivityTimeZone = el.First().ReportedActivityTimeZone,
                                        AmigoTenantTUserId = el.First().AmigoTenantTUserId,
                                        Latitude = el.First().Latitude,
                                        Longitude = el.First().Longitude,
                                        ChargeNo = el.First().ChargeNo,
                                        ActivityTypeName = el.First().ActivityTypeName,
                                        ActivityTypeCode = el.First().ActivityTypeCode,
                                        TractorNumber = el.First().TractorNumber,
                                        FirstName = el.First().FirstName,
                                        LastName = el.First().LastName
                                    }).ToList();


            return ResponseBuilder.Correct(latestPositions);

            //if (latestPositions == null || latestPositions.Count() == 0)
            //{
            //    return ResponseBuilder.Correct(latestPositions);
            //}


            //---------------------------------------------------------------------------------------------
            //----------------  Get driver's firstName/lastName from Identity server ----------------------
            //---------------------------------------------------------------------------------------------


            //string usernamesParameters = string.Empty;
            //foreach (var latestPosition in latestPositions)
            //    usernamesParameters = usernamesParameters + "usernames=" + HttpUtility.UrlEncode(latestPosition.Username) + "&";


            //var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings);
            //var rspUsersDetails = await httpClient.GetAsync("api/Users/GetUsersDetails?" + usernamesParameters);

            //if (rspUsersDetails.IsSuccessStatusCode)
            //{
            //    var usersDetailsJson = await rspUsersDetails.Content.ReadAsStringAsync().ConfigureAwait(false);

            //    var user = JsonConvert.DeserializeObject<ResponseDTO<List<UserResponse>>>(usersDetailsJson);


            //    //----------------------------------------------------------------------
            //    //----------------------    merge with results list -------------------
            //    //----------------------------------------------------------------------
            //    int indexUsername = 0;

            //    foreach (var latestPosition in latestPositions)
            //    {
            //        latestPosition.FirstName = user.Data[indexUsername].FirstName;
            //        latestPosition.LastName = user.Data[indexUsername].LastName;
            //        indexUsername++;
            //    }

            //    return ResponseBuilder.Correct(latestPositions);
            //}
            //else
            //{
            //    throw new Exception("Amigo.Tenant.Application.Services.Tracking - LatestPositionService - SearchAsync - call to IdentityServerHttpClient api/Users/Get was not successful");
            //}
        }

        #region Helpers

        private Expression<Func<LatestPositionDTO, bool>> GetQueryFilter(LatestPositionRequest search)
        {
            Expression<Func<LatestPositionDTO, bool>> queryFilter = p => true;

            if (search.AmigoTenantTUsersIds != null && search.AmigoTenantTUsersIds.Count() > 0)
            {
                queryFilter = queryFilter.And(p => search.AmigoTenantTUsersIds.Contains(p.AmigoTenantTUserId));
            }


            if (!string.IsNullOrEmpty(search.TractorNumber))
            {
                queryFilter = queryFilter.And(p => p.TractorNumber.Contains(search.TractorNumber));
            }

            if (!string.IsNullOrEmpty(search.ShipmentIdOrCostCenterCode))
            {
                queryFilter = queryFilter.And(p => p.ChargeNo.Contains(search.ShipmentIdOrCostCenterCode));
            }

            return queryFilter;
        }

        #endregion


    }


}
