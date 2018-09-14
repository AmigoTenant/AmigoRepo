using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/rentalApplication")]//,CachingMasterData]
    public class RentalApplicationController : ApiController
    {
        private readonly IRentalApplicationApplicationService _rentalApplicationApplicationService;

        public RentalApplicationController(IRentalApplicationApplicationService rentalApplicationApplicationService)
        {
            _rentalApplicationApplicationService = rentalApplicationApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<RentalApplicationSearchDTO>>> Search([FromUri]RentalApplicationSearchRequest search)
        {
            var resp = await _rentalApplicationApplicationService.SearchRentalApplicationAsync(search);
            return resp;            
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<RentalApplicationRegisterRequest>> GetById(int? id)
        {
            var resp = await _rentalApplicationApplicationService.GetByIdAsync(id);
            return resp;
        }

        [HttpPost, Route("register") ]//, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RentalApplicationCreate)]
        public async Task<ResponseDTO> Register(RentalApplicationRegisterRequest rentalApplication)
        {
            if (ModelState.IsValid)
            {
                var response = await _rentalApplicationApplicationService.RegisterRentalApplicationAsync(rentalApplication);
                return response;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RentalApplicationUpdate)]
        public async Task<ResponseDTO> Update(RentalApplicationUpdateRequest rentalApplication)
        {
            if (ModelState.IsValid)
            {
                return await _rentalApplicationApplicationService.UpdateRentalApplicationAsync(rentalApplication);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RentalApplicationDelete)]
        public async Task<ResponseDTO> Delete(RentalApplicationDeleteRequest rentalApplication)
        {
            if (ModelState.IsValid)
            {
                return await _rentalApplicationApplicationService.DeleteRentalApplicationAsync(rentalApplication);
            }
            return ModelState.ToResponse();
        }

        //[HttpGet, Route("searchHouseFeatureDetailRentalApplication")]
        //public async Task<ResponseDTO<List<HouseFeatureDetailRentalApplicationDTO>>> SearchHouseFeatureDetailRentalApplication(int? houseId)
        //{
        //    var resp = await _rentalApplicationApplicationService.SearchHouseFeatureDetailRentalApplicationAsync(houseId);
        //    return resp;
        //}


        /*EXCEL REPORT*/

        //[HttpGet, Route("searchReport"), AllowAnonymous]//ShuttleClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        //public HttpResponseMessage SearchReport([FromUri]RentalApplicationSearchRequest search)
        //{
        //    var response = Request.CreateResponse();
        //    response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
        //        => _rentalApplicationApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
        //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = "rentalApplicationReportDTOs.csv",
        //        DispositionType = "inline"
        //    };
        //    return response;
        //}


        //[HttpPost, Route("changeStatus")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.RentalApplicationUpdate)]
        //public async Task<ResponseDTO> ChangeStatus(RentalApplicationChangeStatusRequest rentalApplication)
        //{
        //    //TODO_: EL CHANGE STATUS SOLO DEBE PASAR EL CONTRACTID Y EL RESTO LLENARLO DESPUES
        //    if (ModelState.IsValid)
        //    {
        //        return await _rentalApplicationApplicationService.ChangeStatusRentalApplicationAsync(rentalApplication);
        //    }
        //    return ModelState.ToResponse();
        //}

    }
}
