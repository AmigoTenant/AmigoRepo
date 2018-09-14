using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;


namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IActivityEventLogApplicationService
    {
        Task<ResponseDTO<PagedList<ActivityEventLogDTO>>> SearchActivityEventLogAll(ActivityEventLogSearchRequest search);
        void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ActivityEventLogSearchRequest search);
    }
}
