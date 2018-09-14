using System.Web.Http;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using System.Collections.Generic;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/latest24Hours")]
    public class Last24HoursController : ApiController
    {

        private readonly ILast24HoursApplicationService _last24HoursService;

        public Last24HoursController(ILast24HoursApplicationService last24HoursService)
        {
            _last24HoursService = last24HoursService;
            _last24HoursService.IdentityServerClientSettings = new ISClientSettings
            {
                SecurityAuthority = Settings.SecurityAuthority,
                ClientId = Settings.IdentityServerClientId,
                ClientSecret = Settings.IdentityServerClientSecret,
                ClientScope = Settings.IdentityServerClientScope
            };
        }


        [HttpGet, Route("search"), Validate]
        public async Task<ResponseDTO<List<Last24HoursDTO>>> Search([FromUri]Last24HoursRequest search)
        {
            var resp = await _last24HoursService.SearchAsync(search);
            return resp;
        }
    }
}
