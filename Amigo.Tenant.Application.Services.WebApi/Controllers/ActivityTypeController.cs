using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/activitytype"), CachingMasterData]
    public class ActivityTypeController : ApiController
    {
        private readonly IActivityTypeApplicationService _activityApplicationService;

        public ActivityTypeController(IActivityTypeApplicationService activityApplicationService)
        {
            _activityApplicationService = activityApplicationService;
        }

        [HttpGet, Route("searchActivitytypeAll")]
        public Task<ResponseDTO<List<ActivityTypeDTO>>> SearchActivityTypeAll()
        {
            var resp = _activityApplicationService.SearchActivityTypeAll();
            return resp;
        }

    }
}
