using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{

    public interface IReportApplicationService
    {
        Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalHistoryAsync(ReportHistoryRequest search);

        Task<ResponseDTO<PagedList<InternalReportDTO>>> SearchInternalCurrentAsync(ReportCurrentRequest search);

        Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalHistoryAsync(ReportHistoryRequest search);

        Task<ResponseDTO<PagedList<ExternalReportDTO>>> SearchExternalCurrentAsync(ReportCurrentRequest search);

        #region Export to excel
        void ProccessExcelToHistory(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ReportHistoryRequest search, string type, bool isExportForDow);
        void ProccessExcelToCurrent(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ReportCurrentRequest search, string type, bool isExportForDow);
        #endregion
    }

}
