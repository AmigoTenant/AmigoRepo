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
    [RoutePrefix("api/equipmentstatus"), CachingMasterData]
    public class EquipmentStatusController : ApiController
    {
        private readonly IEquipmentStatusApplicationService _statusApplicationService;

        public EquipmentStatusController(IEquipmentStatusApplicationService statusApplicationService)
        {
            _statusApplicationService = statusApplicationService;
        }


        [HttpPost, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<EquipmentStatusDTO>>> Search(EquipmentStatusSearchRequest search)
        {
            var resp = await _statusApplicationService.SearchEquipmentStatusByNameAsync(search);
            return resp;
        }

        [HttpGet, Route("searchEquipmentstatusAll")]
        public Task<ResponseDTO<List<EquipmentStatusDTO>>> GetAllAll()
        {
            var resp = _statusApplicationService.SearchEquipmentStatusAll();
            return resp;
        }


    }
}
