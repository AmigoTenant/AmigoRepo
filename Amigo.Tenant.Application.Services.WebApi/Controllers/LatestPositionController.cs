using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/latestPosition")]
    public class LatestPositionController : ApiController
    {

        private readonly ILatestPositionApplicationService _latestPositionService;

        public LatestPositionController(ILatestPositionApplicationService latestPositionService)
        {
            _latestPositionService = latestPositionService;
            _latestPositionService.IdentityServerClientSettings = new ISClientSettings
            {
                SecurityAuthority = Settings.SecurityAuthority,
                ClientId = Settings.IdentityServerClientId,
                ClientSecret = Settings.IdentityServerClientSecret,
                ClientScope = Settings.IdentityServerClientScope
            };
        }


        [HttpGet, Route("search"), Validate]
        public async Task<ResponseDTO<List<LatestPositionDTO>>> Search([FromUri]LatestPositionRequest search)
        {
            var resp = await _latestPositionService.SearchAsync(search);
            return resp;
        }
    }
}

