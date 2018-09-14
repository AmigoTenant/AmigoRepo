

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Common.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Move;

namespace Amigo.Tenant.Application.Services.Interfaces.Move
{
    public interface IAmigoTenanttServiceApplicationService
    {
        Task<ResponseDTO<int>> RegisterAmigoTenanttServiceAsync(AmigoTenanttServiceDTO maintenance);
        Task<ResponseDTO> UpdateAmigoTenantServiceAsync(UpdateAmigoTenantServiceRequest maintenance);
        Task<ResponseDTO<PagedList<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceAsync(AmigoTenantTServiceSearchRequest search);
        Task<ResponseDTO<AmigoTenantTServiceReportDTO>> SearchByIdAsync(int serviceId);

        /********************** FOR APPROVE **********************/
        /********************** FOR APPROVE **********************/
        /********************** FOR APPROVE **********************/

        Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceForApproveAsync(AmigoTenantTServiceApproveSearchRequest search);

        Task<ResponseDTO<PagedListServices<AmigoTenantTServiceReportDTO>>> SearchAmigoTenantTServiceByChargeNumberAsync(AmigoTenantTServiceSearchChargeNumRequest search, int? driverId, int amigoTenantTServiceDateRangeDays);
        Task<ResponseDTO> UpdateAmigoTenantTServiceForApproveAsync(AmigoTenantTServiceRequest maintenance);
        Task<ResponseDTO> ApproveAmigoTenantTServiceAsync(AmigoTenantTServiceApproveRequest maintenance);
        /********************** FOR ACKNOWLEDGE **********************/
        /********************** FOR ACKNOWLEDGE **********************/
        /********************** FOR ACKNOWLEDGE **********************/
        Task<ResponseDTO> UpdateAmigoTenantTServiceAckAsync(UpdateAmigoTenantTServiceAckRequest maintenance, string userName, int userId);

        /********************** FOR EXPORT TO EXCEL **********************/
        void GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, AmigoTenantTServiceSearchRequest search);
        Task<ResponseDTO> CancelAmigoTenantServiceAsync(CancelAmigoTenantServiceRequest maintenance);
    }
}
