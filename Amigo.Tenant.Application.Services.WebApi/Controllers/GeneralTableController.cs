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

    [RoutePrefix("api/generalTable")]
    public class GeneralTableController : ApiController
    {

        private readonly IGeneralTableApplicationService _generalTableApplicationService;

        public GeneralTableController(IGeneralTableApplicationService generalTableApplicationService)
        {
            _generalTableApplicationService = generalTableApplicationService;
        }


        //[HttpPost, Route("searchCriteria")]
        //public async Task<ResponseDTO<PagedList<GeneralTableDTO>>> Search(GeneralTableSearchRequest search)
        //{
        //    var resp = await _generalTableApplicationService.SearchGeneralTableByNameAsync(search);
        //    return resp;
        //}

        [HttpGet, Route("getGeneralTableByTableNameAll"), CachingMasterData]
        public Task<ResponseDTO<List<GeneralTableDTO>>> GetGeneralTableByTableNameAsync(string tableName)
        {
            var resp = _generalTableApplicationService.GetGeneralTableByTableNameAsync(tableName);
            return resp;
        }

        

        //[HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.GeneralTableCreate)]
        //public async Task<ResponseDTO> Register(GeneralTableDTO generalTable)
        //{
        //    return await _generalTableApplicationService.RegisterModuleAsync(generalTable);
        //}

        //[HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.GeneralTableUpdate)]
        //public async Task<ResponseDTO> Update(GeneralTableDTO generalTable)
        //{
        //    return await _generalTableApplicationService.UpdateGeneralTableAsync(generalTable);
        //}

        //[HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.GeneralTableDelete)]
        //public async Task<ResponseDTO> Delete(GeneralTableDTO generalTable)
        //{
        //    return await _generalTableApplicationService.DeleteGeneralTableAsync(generalTable);
        //}
    }
}
