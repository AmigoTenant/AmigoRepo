using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Common;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/modules"),CachingMasterData]
    public class ModuleController : ApiController
    {
        private readonly IModuleApplicationService _modulesApplicationService;

        public ModuleController(IModuleApplicationService modulesApplicationService)
        {
            _modulesApplicationService = modulesApplicationService;
        }

        [HttpGet, Route("search"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ModuleSearch)]
        public async Task<ResponseDTO<PagedList<ModuleDTO>>> Search([FromUri]ModuleSearchRequest search)
        {
                var resp = await _modulesApplicationService.SearchModulesAsync(search);
                return resp;
            
        }

        [HttpGet, Route("get")]
        public async Task<ResponseDTO<ModuleActionsDTO>> Get([FromUri]GetModuleRequest getRequest)
        {

            var resp = await _modulesApplicationService.GetModuleAsync(new GetModuleRequest { Code = getRequest.Code });
            return resp;

        }


        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ModuleCreate)]
        public async Task<ResponseDTO> Register(RegisterModuleRequest module)
        {
            if (ModelState.IsValid)
            {
                return await _modulesApplicationService.RegisterModuleAsync(module);
            }
            return ModelState.ToResponse();
        }


        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ModuleUpdate)]
        public async Task<ResponseDTO> Update(UpdateModuleRequest module)
        {
            if (ModelState.IsValid)
            {
                return await _modulesApplicationService.UpdateModuleAsync(module);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ModuleDelete)]
        public async Task<ResponseDTO> Delete(DeleteModuleRequest module)
        {
            if (ModelState.IsValid)
            {
                return await _modulesApplicationService.DeleteModuleAsync(module);
            }
            return ModelState.ToResponse();
        }

    }
}
