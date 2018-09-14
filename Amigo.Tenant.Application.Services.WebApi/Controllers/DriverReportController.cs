using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using Amigo.Tenant.Common;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/driverParReport")]
    public class DriverReportController : ApiController
    {

        private readonly IDriverReportApplicationService _serviceApplicationService;

        public DriverReportController(IDriverReportApplicationService serviceApplicationService)
        {
            _serviceApplicationService = serviceApplicationService;
            _serviceApplicationService.IdentityServerClientSettings = new ISClientSettings
            {
                SecurityAuthority = Settings.SecurityAuthority,
                ClientId = Settings.IdentityServerClientId,
                ClientSecret = Settings.IdentityServerClientSecret,
                ClientScope = Settings.IdentityServerClientScope
            };
        }


        [HttpGet, Route("search"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.PayReportSearch)]
        public async Task<ResponseDTO<PagedList<DriverPayReportDTO>>> Search([FromUri]DriverPayReportSearchRequest search)
        {
            return await _serviceApplicationService.SearchDriverPayReportAsync(search);
        }

        [HttpGet, Route("searchReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchReport([FromUri]DriverPayReportSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _serviceApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "DriverPayReport.csv",
                DispositionType = "inline"
            };
            return response;
        }
    }
}
