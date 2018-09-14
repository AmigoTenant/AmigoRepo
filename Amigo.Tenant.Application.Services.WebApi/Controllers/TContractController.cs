using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Caching.Web.Filters;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;
using Amigo.Tenant.Common;
using static Amigo.Tenant.Common.ConstantsSecurity;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{    
    [RoutePrefix("api/contract")]//,CachingMasterData]
    public class ContractController : ApiController
    {
        private readonly IContractApplicationService _contractApplicationService;

        public ContractController(IContractApplicationService contractApplicationService)
        {
            _contractApplicationService = contractApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<ContractSearchDTO>>> Search([FromUri]ContractSearchRequest search)
        {
            var resp = await _contractApplicationService.SearchContractAsync(search);
            return resp;            
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<ContractRegisterRequest>> GetById(int? id)
        {
            var resp = await _contractApplicationService.GetByIdAsync(id);
            return resp;
        }

        [HttpPost, Route("register") ]//, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ContractCreate)]
        public async Task<ResponseDTO> Register(ContractRegisterRequest contract)
        {
            if (ModelState.IsValid)
            {
                var response = await _contractApplicationService.RegisterContractAsync(contract);
                return response;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ContractUpdate)]
        public async Task<ResponseDTO> Update(ContractUpdateRequest contract)
        {
            if (ModelState.IsValid)
            {
                return await _contractApplicationService.UpdateContractAsync(contract);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ContractDelete)]
        public async Task<ResponseDTO> Delete(ContractDeleteRequest contract)
        {
            if (ModelState.IsValid)
            {
                return await _contractApplicationService.DeleteContractAsync(contract);
            }
            return ModelState.ToResponse();
        }

        [HttpGet, Route("searchHouseFeatureDetailContract")]
        public async Task<ResponseDTO<List<HouseFeatureDetailContractDTO>>> SearchHouseFeatureDetailContract(int? houseId)
        {
            var resp = await _contractApplicationService.SearchHouseFeatureDetailContractAsync(houseId);
            return resp;
        }


        /*EXCEL REPORT*/

        [HttpGet, Route("searchReport"), AllowAnonymous]//ShuttleClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchReport([FromUri]ContractSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _contractApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "contractReportDTOs.csv",
                DispositionType = "inline"
            };
            return response;
        }


        [HttpPost, Route("changeStatus")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ContractUpdate)]
        public async Task<ResponseDTO> ChangeStatus(ContractChangeStatusRequest contract)
        {
            //TODO_: EL CHANGE STATUS SOLO DEBE PASAR EL CONTRACTID Y EL RESTO LLENARLO DESPUES
            if (ModelState.IsValid)
            {
                return await _contractApplicationService.ChangeStatusContractAsync(contract);
            }
            return ModelState.ToResponse();
        }

    }
}
