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
    [RoutePrefix("api/dspatchingParty"),CachingMasterData]
    public class DispatchingPartyController : ApiController
    {

        private readonly IDispatchingPartyApplicationService _DispatchingPartyApplicationService;

        public DispatchingPartyController(IDispatchingPartyApplicationService DispatchingPartyApplicationService)
        {
            _DispatchingPartyApplicationService = DispatchingPartyApplicationService;
        }

        [HttpGet,Route("search")]
        public async Task<ResponseDTO<PagedList<DispatchingPartyDTO>>> Search([FromUri]DispatchingPartySearchRequest search)
        {
            var resp = await _DispatchingPartyApplicationService.SearchDispatchingPartyAsync(search);
            return resp;
        }

        [HttpGet, Route("searchDispatchingPartyAll")]
        public async Task<ResponseDTO<List<DispatchingPartyDTO>>> GetAll()
        {
            var resp = await _DispatchingPartyApplicationService.GetAllAsync();
            return resp;
        }
    }
}
