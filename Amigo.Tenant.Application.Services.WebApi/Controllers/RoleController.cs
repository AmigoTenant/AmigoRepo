using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Common;
using Amigo.Tenant.Security.Permissions.Abstract;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/AmigoTenantTRole"),CachingMasterData]
    public class AmigoTenantTRoleController : ApiController
    {
        private readonly IAmigoTenantTRoleApplicationService _AmigoTenantTRoleApplicationService;

        public AmigoTenantTRoleController(IAmigoTenantTRoleApplicationService AmigoTenantTRoleApplicationService)
        {
            _AmigoTenantTRoleApplicationService = AmigoTenantTRoleApplicationService;
        }

        [HttpGet, Route("searchCriteria"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RoleSearch)]
        public async Task<ResponseDTO<PagedList<AmigoTenantTRoleDTO>>> Search([FromUri] AmigoTenantTRoleSearchRequest search)
        {
            var resp = await _AmigoTenantTRoleApplicationService.SearchAmigoTenantTRoleByCriteriaAsync(search).ConfigureAwait(false);
            return resp;
        }

        [HttpGet, Route("CustomRoleSearch")]
        public async Task<ResponseDTO<PagedList<AmigoTenantTRoleDTO>>> CustomRoleSearch([FromUri] AmigoTenantTRoleSearchRequest search)
        {
            var resp = await _AmigoTenantTRoleApplicationService.SearchAmigoTenantTRoleByCriteriaAsync(search).ConfigureAwait(false);
            return resp;
        }

        [HttpGet, Route("searchBasicCriteria")]
        public async Task<ResponseDTO<PagedList<AmigoTenantTRoleBasicDTO>>> SearchById(
            [FromUri] AmigoTenantTRoleSearchRequest search)
        {
            var resp = await _AmigoTenantTRoleApplicationService.SearchAmigoTenantTRoleBasicCriteriaAsync(search).ConfigureAwait(false);
            return resp;
        }


        [HttpGet, Route("getRolTree")]
        public async Task<ResponseDTO<List<ModuleTreeDTO>>> GetRol([FromUri] string code)
        {
            var resp = await _AmigoTenantTRoleApplicationService.GetRoleTree(code).ConfigureAwait(false);
            return resp;
        }

        [HttpGet, Route("getModuleAction")]
        public async Task<ResponseDTO<List<ModuleTreeDTO>>> GetModuleAction()
        {
            var resp = await _AmigoTenantTRoleApplicationService.GetModuleActionAsyn().ConfigureAwait(false);
            return resp;
        }


        [HttpGet, Route("exists")]
        public async Task<bool> Exists([FromUri] AmigoTenantTRoleSearchRequest search)
        {
            var resp = await _AmigoTenantTRoleApplicationService.Exists(search).ConfigureAwait(false);
            return resp;
        }

        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RoleCreate)]
        public async Task<ResponseDTO> Register(AmigoTenantTRoleDTO dto)
        {
            if (ModelState.IsValid)
            {
                var permssions = (IPermissionsReader)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionsReader));
                var result= await _AmigoTenantTRoleApplicationService.Register(dto).ConfigureAwait(false);
                await permssions.UpdatePermissionsCache().ConfigureAwait(false);
                return result;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RoleUpdate)]
        public async Task<ResponseDTO> Update(AmigoTenantTRoleDTO dto)
        {
            if (ModelState.IsValid)
            {
                var permssions = (IPermissionsReader)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionsReader));
                var result = await _AmigoTenantTRoleApplicationService.Update(dto).ConfigureAwait(false);
                await permssions.UpdatePermissionsCache().ConfigureAwait(false);
                return result;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("updateActions")]
        public async Task<ResponseDTO> UpdateActions(AmigoTenanttRolPermissionRequest actions)
        {

            return await _AmigoTenantTRoleApplicationService.UpdateActions(actions).ConfigureAwait(false);
        }


        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RoleDelete)]
        public async Task<ResponseDTO> Delete(AmigoTenantTRoleStatusDTO dto)
        {
            return await _AmigoTenantTRoleApplicationService.Delete(dto).ConfigureAwait(false);
        }


        [HttpGet,AllowAnonymous,Route("permissions")]
        public async Task<HttpResponseMessage> Permissions()
        {
            var permssions = (IPermissionsReader)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionsReader));
            var p = await permssions.GetAllPermissionsWithActionsAsync().ConfigureAwait(false);
            var projected = p.Select(x => new { Action = x.Action.Name, x.ActionId,Role= x.AmigoTenantTRole.Name }).ToList();
            var json = JsonConvert.SerializeObject(projected);
            return new HttpResponseMessage
            {
                Content = new StringContent(json)                
            };
        }

        [HttpGet, AllowAnonymous, Route("permissions/{role}")]
        public async Task<HttpResponseMessage> Permissions(string role)
        {
            var permssions = (IPermissionsReader)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionsReader));
            var p = await permssions.GetAllPermissionsWithActionsAsync().ConfigureAwait(false);
            var projected = p.Where(x=> x.AmigoTenantTRole.Name == role).Select(x => new { Action = x.Action.Name, x.ActionId, Role = x.AmigoTenantTRole.Name }).ToList();
            var json = JsonConvert.SerializeObject(projected);
            return new HttpResponseMessage
            {
                Content = new StringContent(json)
            };
        }
    }
}