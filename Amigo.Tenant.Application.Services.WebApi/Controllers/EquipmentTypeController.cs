using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Caching.Web.Filters;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/equipmenttype"), CachingMasterData]
    public class EquipmentTypeController : ApiController
    {
        private readonly IEquipmentTypeApplicationService _equipmentTypeApplicationService;

        public EquipmentTypeController(IEquipmentTypeApplicationService equipmentTypeApplicationService)
        {
            _equipmentTypeApplicationService = equipmentTypeApplicationService;
        }

        [HttpPost, Route("search")]
        public async Task<ResponseDTO<PagedList<EquipmentTypeDTO>>> Search(EquipmentTypeSearchRequest search)
        {
            var resp = await _equipmentTypeApplicationService.SearchEquipmentTypeAsync(search);
            return resp;
        }
        //Sample:: ,AmigoTenantClaimsAuthorize(ActionCode = "100")
        [HttpGet, Route("searchEquipmenttypeAll")]//, AmigoTenantClaimsAuthorize(ActionCode = "100")]
        public Task<ResponseDTO<List<EquipmentTypeDTO>>> GetAllAll()
        {
            var resp = _equipmentTypeApplicationService.SearchEquipmentType();
            return resp;
        }


        public IHttpActionResult Put()
        {
            return Ok();
        }
    }
}
