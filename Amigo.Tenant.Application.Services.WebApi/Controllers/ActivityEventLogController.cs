using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/activityEvenLog")]
    public class ActivityEventLogController : ApiController
    {
        private readonly IActivityEventLogApplicationService _activityEventLogApplicationService;

        public ActivityEventLogController(IActivityEventLogApplicationService activityEventLogApplicationService)
        {
            _activityEventLogApplicationService = activityEventLogApplicationService;
        }

        [HttpGet, Route("SearchActivityEventLogAll")]
        public Task<ResponseDTO<PagedList<ActivityEventLogDTO>>> SearchActivityEventLogAll([FromUri]ActivityEventLogSearchRequest search)
        {
            var resp = _activityEventLogApplicationService.SearchActivityEventLogAll(search);
            return resp;
        }

        [HttpGet, Route("SearchActivityEventLogAllReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchActivityEventLogAllReport([FromUri]ActivityEventLogSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _activityEventLogApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "ActivityLog.csv",
                DispositionType = "inline"
            };
            return response;
        }
    }
}
