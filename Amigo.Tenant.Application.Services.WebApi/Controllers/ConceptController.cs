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
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{

    [RoutePrefix("api/concept")]
    public class ConceptController : ApiController
    {

        private readonly IConceptApplicationService _conceptApplicationService;

        public ConceptController(IConceptApplicationService conceptApplicationService)
        {
            _conceptApplicationService = conceptApplicationService;
        }

        [HttpGet, Route("getConceptsByTypeCode")] //, CachingMasterData]
        public Task<ResponseDTO<List<ConceptDTO>>> GetConceptsByTypeCode(string code)
        {
            var resp = _conceptApplicationService.GetConceptsByTypeCodeAsync(code);
            return resp;
        }

        [HttpGet, Route("getConceptByTypeIdList")]
        public async Task<ResponseDTO<List<ConceptDTO>>> getConceptByTypeIdList([FromUri] List<string> idList)
        {
            var resp = await _conceptApplicationService.GetConceptByTypeIdListAsync(idList);
            return resp;
        }

        //[HttpPost, Route("searchCriteria")]
        //public async Task<ResponseDTO<PagedList<ConceptDTO>>> Search(ConceptSearchRequest search)
        //{
        //    var resp = await _conceptApplicationService.SearchConceptByNameAsync(search);
        //    return resp;
        //}

        //[HttpGet, Route("searchConceptAll"), CachingMasterData]
        //public Task<ResponseDTO<List<ConceptDTO>>> SearchConceptAll()
        //{
        //    var resp = _conceptApplicationService.SearchConceptAll();
        //    return resp;
        //}

        //[HttpGet, Route("searchConceptAllTypeAheadByName"), CachingMasterData]
        //public Task<ResponseDTO<List<ConceptTypeAheadDTO>>> GetConceptAllTypeAheadByName(string name)
        //{
        //    var resp = _conceptApplicationService.SearchConceptAllTypeAhead(name);
        //    return resp;
        //}

        //[HttpGet, Route("searchConceptByName"), CachingMasterData]
        //public async Task<ConceptTypeAheadDTO> SearchByName([FromUri]string name)
        //{
        //    var resp = await _conceptApplicationService.SearchConceptByNameAsync(name);
        //    return resp;
        //}

        //[HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ConceptCreate)]
        //public async Task<ResponseDTO> Register(ConceptDTO concept)
        //{
        //    return await _conceptApplicationService.RegisterModuleAsync(concept);
        //}

        //[HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ConceptUpdate)]
        //public async Task<ResponseDTO> Update(ConceptDTO concept)
        //{
        //    return await _conceptApplicationService.UpdateConceptAsync(concept);
        //}

        //[HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ConceptDelete)]
        //public async Task<ResponseDTO> Delete(ConceptDTO concept)
        //{
        //    return await _conceptApplicationService.DeleteConceptAsync(concept);
        //}
    }
}
