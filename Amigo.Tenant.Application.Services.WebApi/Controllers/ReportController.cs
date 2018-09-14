using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using System.Web.Http;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;
using System;
using System.Web;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{

    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {

        private readonly IReportApplicationService _reportService;

        public ReportController(IReportApplicationService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet, Route("internalHistory"), Validate]
        public async Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalHistory([FromUri]ReportHistoryRequest search)
        {
            var resp = await _reportService.SearchInternalHistoryAsync(search);
            return resp;
        }

        [HttpGet, Route("internalHistoryReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchInternalHistoryReport([FromUri]ReportHistoryRequest search)
        {
            var response = Request.CreateResponse();
            string type = "internalHistoryReport";
            bool isExportForDow = false;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _reportService.ProccessExcelToHistory(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "_Report.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpGet, Route("internalHistoryReportForDow"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchInternalHistoryReportForDow([FromUri]ReportHistoryRequest search)
        {
            var response = Request.CreateResponse();
            string type = "internalHistoryReport";
            bool isExportForDow = true;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _reportService.ProccessExcelToHistory(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "_Report.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpGet, Route("internalCurrent"), Validate]
        public async Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalCurrent([FromUri]ReportCurrentRequest search)
        {
            var resp = await _reportService.SearchInternalCurrentAsync(search);
            return resp;
        }

        [HttpGet, Route("internalCurrentReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchInternalCurrentReport([FromUri]ReportCurrentRequest search)
        {
            var response = Request.CreateResponse();
            string type = "internalCurrentReport";
            bool isExportForDow = false;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _reportService.ProccessExcelToCurrent(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "_Report.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpGet, Route("internalCurrentReportForDow"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchInternalCurrentReportForDow([FromUri]ReportCurrentRequest search)
        {
            var response = Request.CreateResponse();
            string type = "internalCurrentReport";
            bool isExportForDow = true;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _reportService.ProccessExcelToCurrent(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "_ReportForDow.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpGet, Route("externalHistory"), Validate]
        public async Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalHistory([FromUri]ReportHistoryRequest search)
        {
            var resp = await _reportService.SearchExternalHistoryAsync(search);
            return resp;
        }

        [HttpGet, Route("externalHistoryReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchExternalHistoryReport([FromUri]ReportHistoryRequest search)
        {
            var response = Request.CreateResponse();
            string type = "externalHistoryReport";
            bool isExportForDow = false;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _reportService.ProccessExcelToHistory(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "_Report.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpGet, Route("externalCurrent"), Validate]
        public async Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalCurrent([FromUri]ReportCurrentRequest search)
        {
            var resp = await _reportService.SearchExternalCurrentAsync(search);
            return resp;
        }

        [HttpGet, Route("externalCurrentReport"), AllowAnonymous]//AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchExternalCurrentReport([FromUri]ReportCurrentRequest search)
        {
            try
            {
                var response = Request.CreateResponse();
                string type = "externalCurrentReport";
                bool isExportForDow = false;
                response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                    => _reportService.ProccessExcelToCurrent(outputStream, httpContent, transportContext, search, type, isExportForDow), new MediaTypeHeaderValue("text/csv"));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "_Report.csv",
                    DispositionType = "inline",

                };
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}
