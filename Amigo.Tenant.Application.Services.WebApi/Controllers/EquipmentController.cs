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
    [RoutePrefix("api/equipment")]
    public class EquipmentController : ApiController
    {

        private readonly IEquipmentApplicationService _EquipmentApplicationService;

        public EquipmentController(IEquipmentApplicationService EquipmentApplicationService)
        {
            _EquipmentApplicationService = EquipmentApplicationService;
        }

        [HttpGet, Route("search")]
        public async Task<ResponseDTO<PagedList<EquipmentDTO>>> Search([FromUri]EquipmentSearchRequest search)
        {
            var resp = await _EquipmentApplicationService.SearchEquipmentAsync(search);
            return resp;
        }


    }
}
