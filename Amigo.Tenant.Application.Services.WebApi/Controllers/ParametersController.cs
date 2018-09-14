using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Caching.Web.Filters;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
   [RoutePrefix("api/parameters"),CachingMasterData]
    public class ParametersController : ApiController
    {
        private readonly IAmigoTenantParameterApplicationService _amigoTenantParameterService;

        public ParametersController(IAmigoTenantParameterApplicationService amigoTenantParameterService)
        {
            _amigoTenantParameterService = amigoTenantParameterService;
        }

        [HttpGet, Route("GetAmigoTenantParameters")]
        public async Task<ResponseDTO<List<AmigoTenantParameterDTO>>> GetAmigoTenantParameters()
        {
            var amigoTenantParameter = await _amigoTenantParameterService.GetAmigoTenantParameters();
            return amigoTenantParameter;
            
        }

        [HttpGet, Route("SearchParametersForMobile"), Authorize(Roles = "DRIVER")]
        public async Task<ResponseDTO<List<CustomAmigoTenantParameterDTO>>> SearchParametersForMobile()
        {
            var amigoTenantParameter = await _amigoTenantParameterService.SearchParametersForMobile();
            return amigoTenantParameter;

        }

    }
}
