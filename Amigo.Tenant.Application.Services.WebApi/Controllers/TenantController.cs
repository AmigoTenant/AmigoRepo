using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Caching.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/tenant"), CachingMasterData]
    public class TenantController : ApiController
    {
        private readonly IMainTenantApplicationService _tenantApplicationService;

        public TenantController(IMainTenantApplicationService tenantApplicationService)
        {
            _tenantApplicationService = tenantApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<MainTenantDTO>>> Search([FromUri]MainTenantSearchRequest search)
        {
            var resp = await _tenantApplicationService.SearchTenantAsync(search);
            return resp;
        }

        [HttpGet, Route("searchByCodeName")]
        public async Task<ResponseDTO<PagedList<MainTenantBasicDTO>>> SearchByCodeAndName([FromUri]MainTenantCodeNameRequest search)
        {
            var resp = await _tenantApplicationService.SearchByCodeAndName(search);
            return resp;
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<MainTenantBasicDTO>> GetById(int? id, int? typeId)
        {
            var resp = await _tenantApplicationService.GetByIdTypeId(id, typeId);
            return resp;
        }

        [HttpGet, Route("getTenantById")]
        public async Task<ResponseDTO<MainTenantDTO>> getTenantById(int? id)
        {
            var resp = await _tenantApplicationService.GetById(id);
            return resp;
        }

        [HttpGet, Route("searchForTypeAhead")]
        public async Task<ResponseDTO<List<MainTenantBasicDTO>>> SearchForTypeAhead(string search, bool validateInActiveContract)
        {
            var resp = await _tenantApplicationService.SearchForTypeAhead(search, validateInActiveContract);
            return resp;
        }

        [HttpPost, Route("register")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.TenantCreate)]
        public async Task<ResponseDTO> Register(RegisterMainTenantRequest tenant)
        {
            if (ModelState.IsValid)
            {
                return await _tenantApplicationService.RegisterMainTenantAsync(tenant);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.TenantUpdate)]
        public async Task<ResponseDTO> Update(UpdateMainTenantRequest tenant)
        {
            if (ModelState.IsValid)
            {
                return await _tenantApplicationService.UpdateMainTenantAsync(tenant);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ContractDelete)]
        public async Task<ResponseDTO> Delete(DeleteMainTenantRequest tenant)
        {
            if (ModelState.IsValid)
            {
                return await _tenantApplicationService.DeleteMainTenantAsync(tenant);
            }
            return ModelState.ToResponse();
        }

    }
}
