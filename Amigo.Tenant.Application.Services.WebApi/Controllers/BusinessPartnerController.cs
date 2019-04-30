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

    [RoutePrefix("api/businessPartner")]
    public class BusinessPartnerController : ApiController
    {

        private readonly IBusinessPartnerApplicationService _businessPartnerApplicationService;

        public BusinessPartnerController(IBusinessPartnerApplicationService businessPartnerApplicationService)
        {
            _businessPartnerApplicationService = businessPartnerApplicationService;
        }


        [HttpGet, Route("getBusinessPartnerByName")] //, CachingMasterData]
        public Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByName(string name, string bpTypeCode)
        {
            var resp = _businessPartnerApplicationService.GetBusinessPartnerByNameAsync(name, bpTypeCode);
            return resp;
        }

        [HttpGet, Route("getBusinessPartnerByBPType")] //, CachingMasterData]
        public Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByBPTypeCode(string bpTypeCode)
        {
            var resp = _businessPartnerApplicationService.GetBusinessPartnerByBPTypeCodeAsync(bpTypeCode);
            return resp;
        }

    }
}
