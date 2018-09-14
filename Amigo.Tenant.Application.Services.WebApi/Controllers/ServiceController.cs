
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
    [RoutePrefix("api/service"), CachingMasterData]
    public class ServiceController : ApiController
    {

        private readonly IServiceApplicationService _serviceApplicationService;

        public ServiceController(IServiceApplicationService serviceApplicationService)
        {
            _serviceApplicationService = serviceApplicationService;
        }


        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<ServiceDTO>>> Search(ServiceSearchRequest search)
        {
            var resp = await _serviceApplicationService.SearchServiceByNameAsync(search);
            return resp;
        }

        [HttpGet, Route("searchServiceAll")]
        public Task<ResponseDTO<List<ServiceDTO>>> GetServiceAll()
        {
            var resp = _serviceApplicationService.GetServiceAll();
            return resp;
        }




    }
}
