using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IDriverReportApplicationService
    {
        ISClientSettings IdentityServerClientSettings { get; set; }
        Task<ResponseDTO<PagedList<DriverPayReportDTO>>> SearchDriverPayReportAsync(DriverPayReportSearchRequest search);
        void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, DriverPayReportSearchRequest search);
    }
}