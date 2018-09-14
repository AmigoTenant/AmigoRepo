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

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{

    [RoutePrefix("api/costcenter")]
    public class CostCenterController : ApiController
    {

        private readonly ICostCenterApplicationService _costCenterApplicationService;

        public CostCenterController(ICostCenterApplicationService costCenterApplicationService)
        {
            _costCenterApplicationService = costCenterApplicationService;
        }


        [HttpPost, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<CostCenterDTO>>> Search(CostCenterSearchRequest search)
        {
            var resp = await _costCenterApplicationService.SearchCostCenterByNameAsync(search);
            return resp;
        }

        [HttpGet, Route("searchCostcenterAll"), CachingMasterData]
        public Task<ResponseDTO<List<CostCenterDTO>>> SearchCostCenterAll()
        {
            var resp = _costCenterApplicationService.SearchCostCenterAll();
            return resp;
        }

        [HttpGet, Route("searchCostCenterAllTypeAheadByName"), CachingMasterData]
        public Task<ResponseDTO<List<CostCenterTypeAheadDTO>>> GetCostCenterAllTypeAheadByName(string name)
        {
            var resp = _costCenterApplicationService.SearchCostCenterAllTypeAhead(name);
            return resp;
        }

        [HttpGet, Route("searchCostCenterByName"), CachingMasterData]
        public async Task<CostCenterTypeAheadDTO> SearchByName([FromUri]string name)
        {
            var resp = await _costCenterApplicationService.SearchCostCenterByNameAsync(name);
            return resp;
        }

        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.CostCenterCreate)]
        public async Task<ResponseDTO> Register(CostCenterDTO costCenter)
        {
            return await _costCenterApplicationService.RegisterModuleAsync(costCenter);
        }

        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.CostCenterUpdate)]
        public async Task<ResponseDTO> Update(CostCenterDTO costCenter)
        {
            return await _costCenterApplicationService.UpdateCostCenterAsync(costCenter);
        }

        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.CostCenterDelete)]
        public async Task<ResponseDTO> Delete(CostCenterDTO costCenter)
        {
            return await _costCenterApplicationService.DeleteCostCenterAsync(costCenter);
        }
    }
}
