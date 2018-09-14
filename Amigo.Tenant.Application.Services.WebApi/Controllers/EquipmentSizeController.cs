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
    [RoutePrefix("api/equipmentsize"),CachingMasterData]
    public class EquipmentSizeController : ApiController
    {

        private readonly IEquipmentSizeApplicationService _equipmentSizeApplicationService;

        public EquipmentSizeController(IEquipmentSizeApplicationService equipmentSizeApplicationService)
        {
            _equipmentSizeApplicationService = equipmentSizeApplicationService;
        }

        [HttpPost, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<EquipmentSizeDTO>>> Search(EquipmentSizeSearchRequest search)
        {
            var resp = await _equipmentSizeApplicationService.SearchEquipmentSizeAsync(search);
            return resp;
        }

        [HttpGet, Route("searchEquipmentsizeAll")]
        public Task<ResponseDTO<List<EquipmentSizeDTO>>> GetAllAll()
        {
            var resp = _equipmentSizeApplicationService.SearchEquipmentSizeAll();
            return resp;
        }


    }
}
