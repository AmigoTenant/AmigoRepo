using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Identity;
using Amigo.Tenant.Web.Logging;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/amigotenantteventlog")]
    public class amigoTenanttEventLogController : ApiController
    {
        private readonly IAmigoTenantTEventLogApplicationService _logApplicationService;

        public amigoTenanttEventLogController(IAmigoTenantTEventLogApplicationService logApplicationService)
        {
            _logApplicationService = logApplicationService;
        }


        [HttpPost, Route("registerActivity"), LogRequestActionFilter]
        public async Task<ResponseDTO> Register(AmigoTenantTEventLogDTO maintenance)
        {
            maintenance.IncludeRequestLog = Settings.RequestLogEnabled;
            maintenance.UserId = User.Identity.GetUserId();
            maintenance.Username = User.Identity.GetUsername();
            return await _logApplicationService.RegisterAmigoTenantTEventLogAsync(maintenance);
        }


        [HttpGet, Route("search")]
        public async Task<ResponseDTO<PagedList<AmigoTenantTEventLogSearchResultDTO>>> Search([FromUri]AmigoTenantTEventLogSearchRequest search)
        {
            var resp = await _logApplicationService.SearchAsync(search);
            return resp;
        }

   

    }
}
