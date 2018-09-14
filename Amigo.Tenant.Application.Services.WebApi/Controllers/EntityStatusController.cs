using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Common;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{

    [RoutePrefix("api/entityStatus")]
    public class EntityStatusController : ApiController
    {

        private readonly IEntityStatusApplicationService _entityStatusApplicationService;

        public EntityStatusController(IEntityStatusApplicationService entityStatusApplicationService)
        {
            _entityStatusApplicationService = entityStatusApplicationService;
        }


        //[HttpPost, Route("searchCriteria")]
        //public async Task<ResponseDTO<PagedList<EntityStatusDTO>>> Search(EntityStatusSearchRequest search)
        //{
        //    var resp = await _entityStatusApplicationService.SearchEntityStatusByNameAsync(search);
        //    return resp;
        //}

        [HttpGet, Route("getEntityStatusByEntityCodeAll")]//, CachingMasterData]
        public Task<ResponseDTO<List<EntityStatusDTO>>> GetEntityStatusByEntityCodeAsync(string entityCode)
        {
            var resp = _entityStatusApplicationService.GetEntityStatusByEntityCodeAsync(entityCode);
            return resp;
        }

        

        //[HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.EntityStatusCreate)]
        //public async Task<ResponseDTO> Register(EntityStatusDTO entityStatus)
        //{
        //    return await _entityStatusApplicationService.RegisterModuleAsync(entityStatus);
        //}

        //[HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.EntityStatusUpdate)]
        //public async Task<ResponseDTO> Update(EntityStatusDTO entityStatus)
        //{
        //    return await _entityStatusApplicationService.UpdateEntityStatusAsync(entityStatus);
        //}

        //[HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.EntityStatusDelete)]
        //public async Task<ResponseDTO> Delete(EntityStatusDTO entityStatus)
        //{
        //    return await _entityStatusApplicationService.DeleteEntityStatusAsync(entityStatus);
        //}
    }
}
