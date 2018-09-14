using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Common.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Application.Services.Interfaces.Move;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Identity;
using Amigo.Tenant.Common;
using Amigo.Tenant.Web.Logging;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/move")]
    public class AmigoTenanttServiceController : ApiController
    {

        private readonly IAmigoTenanttServiceApplicationService _serviceApplicationService;
        private readonly IAmigoTenantParameterApplicationService _amigoTenantParameterService;

        public AmigoTenanttServiceController(IAmigoTenanttServiceApplicationService serviceApplicationService, IAmigoTenantParameterApplicationService amigoTenantParameterService)
        {
            _serviceApplicationService = serviceApplicationService;
            _amigoTenantParameterService = amigoTenantParameterService;
        }


        [HttpPost, Route("register"), Authorize(Roles = "DRIVER"), LogRequestActionFilter]
        public async Task<ResponseDTO<int>> Register(AmigoTenanttServiceDTO maintenance)
        {
            ChangingForeignKeyFromZeroToNull(maintenance);
            maintenance.IncludeRequestLog = Settings.RequestLogEnabled;
            maintenance.AmigoTenantTUserId = User.Identity.GetUserId();
            maintenance.Username = User.Identity.GetUsername();
            return await _serviceApplicationService.RegisterAmigoTenanttServiceAsync(maintenance);
        }

        private void ChangingForeignKeyFromZeroToNull(AmigoTenanttServiceDTO maintenance)
        {
            if (maintenance.OriginLocationId.HasValue && maintenance.OriginLocationId == 0)
                maintenance.OriginLocationId = null;
            if (maintenance.DestinationLocationId.HasValue && maintenance.DestinationLocationId == 0)
                maintenance.DestinationLocationId = null;
            if (maintenance.DispatchingPartyId.HasValue && maintenance.DispatchingPartyId == 0)
                maintenance.DispatchingPartyId = null;
            if (maintenance.EquipmentSizeId.HasValue && maintenance.EquipmentSizeId == 0)
                maintenance.EquipmentSizeId = null;
            if (maintenance.EquipmentTypeId.HasValue && maintenance.EquipmentTypeId == 0)
                maintenance.EquipmentTypeId = null;
            if (maintenance.EquipmentStatusId.HasValue && maintenance.EquipmentStatusId == 0)
                maintenance.EquipmentStatusId = null;
            if (maintenance.CostCenterId.HasValue && maintenance.CostCenterId == 0)
                maintenance.CostCenterId = null;
            if (maintenance.ProductId.HasValue && maintenance.ProductId == 0)
                maintenance.ProductId = null;
            if (maintenance.ServiceId.HasValue && maintenance.ServiceId == 0)
                maintenance.ServiceId = null;
            //if (maintenance.AmigoTenantTUserId.HasValue && maintenance.AmigoTenantTUserId == 0)
            //    maintenance.AmigoTenantTUserId = null;

        }

        [HttpPost, Route("update"), Authorize(Roles = "DRIVER"), LogRequestActionFilter]
        public async Task<ResponseDTO> Update(UpdateAmigoTenantServiceRequest maintenance)
        {
            maintenance.IncludeRequestLog = Settings.RequestLogEnabled;
            maintenance.AmigoTenantTUserId = User.Identity.GetUserId();
            return await _serviceApplicationService.UpdateAmigoTenantServiceAsync(maintenance);
        }

        [HttpPost, Route("cancel"), Authorize(Roles = "DRIVER"), LogRequestActionFilter]
        public async Task<ResponseDTO> Cancel(CancelAmigoTenantServiceRequest maintenance)
        {
            maintenance.IncludeRequestLog = Settings.RequestLogEnabled;
            maintenance.AmigoTenantTUserId = User.Identity.GetUserId();
            return await _serviceApplicationService.CancelAmigoTenantServiceAsync(maintenance);
        }

        [HttpGet, Route("search"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)]
        public async Task<ResponseDTO<PagedList<AmigoTenantTServiceReportDTO>>> Search([FromUri]AmigoTenantTServiceSearchRequest search)
        {
            return await _serviceApplicationService.SearchAmigoTenantTServiceAsync(search);
        }

        [HttpGet, Route("searchReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchReport([FromUri]AmigoTenantTServiceSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _serviceApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "amigoTenantTServiceReportDTOs.csv",
                DispositionType = "inline"
            };
            return response;
        }       

        [HttpGet, Route("SearchById")]
        public async Task<ResponseDTO<AmigoTenantTServiceReportDTO>> SearchById(int amigoTenantTServiceId)
        {
            return await _serviceApplicationService.SearchByIdAsync(amigoTenantTServiceId);
        }


        /**************************************************************
        ************* APPROVE *****************************************
        ***************************************************************/
        [HttpGet, Route("searchAmigoTenantTServiceForApprove"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ApprovalSearch)]
        public async Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceForApprove([FromUri]AmigoTenantTServiceApproveSearchRequest search)
        {
            return await _serviceApplicationService.SearchAmigoTenantTServiceForApproveAsync(search);
        }

        /**************************************************************
        ************* Search by Charge Number *****************************************
        ***************************************************************/
        [HttpGet, Route("SearchAmigoTenantTServiceByChargeNumber"), Authorize(Roles = "DRIVER")]
        public async Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceByChargeNumber([FromUri]AmigoTenantTServiceSearchChargeNumRequest search)
        {
            var amigoTenantParameter = await _amigoTenantParameterService.GetAmigoTenantParameterByCodeAsync("AmigoTenantServDateDays");
            var days = Convert.ToInt32(amigoTenantParameter.Value);
            return await _serviceApplicationService.SearchAmigoTenantTServiceByChargeNumberAsync(search, User.Identity.GetUserId(), days);
        }



        [HttpPost, Route("updateAmigoTenantTServiceForApprove"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ApprovalSave)]
        public async Task<ResponseDTO> UpdateAmigoTenantTServiceForApprove(AmigoTenantTServiceRequest maintenance)
        {
            maintenance.UpdatedBy = User.Identity.GetUserId();
            maintenance.UpdatedDate = DateTime.UtcNow;
            //maintenance.HasH34 = maintenance.HasH34;
            return await _serviceApplicationService.UpdateAmigoTenantTServiceForApproveAsync(maintenance);
        }



        [HttpPost, Route("approve"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ApprovalApprove)]
        public async Task<ResponseDTO> Approve(AmigoTenantTServiceApproveRequest search)
        {
            return await _serviceApplicationService.ApproveAmigoTenantTServiceAsync(search);
        }



        /**************************************************************
        ************* AKNOWLEDGE **************************************
        ***************************************************************/
        [HttpPost, Route("updateAck"), Authorize(Roles = "DRIVER"), LogRequestActionFilter]
        public async Task<ResponseDTO> UpdateAck(UpdateAmigoTenantTServiceAckRequest maintenance)
        {
            maintenance.IncludeRequestLog = Settings.RequestLogEnabled;
            maintenance.UpdatedBy = User.Identity.GetUserId();
            return await _serviceApplicationService.UpdateAmigoTenantTServiceAckAsync(maintenance, User.Identity.GetUsername(), User.Identity.GetUserId());
        }


    }
}
