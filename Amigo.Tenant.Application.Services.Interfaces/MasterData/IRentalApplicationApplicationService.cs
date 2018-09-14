using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IRentalApplicationApplicationService 
    {
        Task<ResponseDTO> RegisterRentalApplicationAsync(RentalApplicationRegisterRequest request);
        Task<ResponseDTO> UpdateRentalApplicationAsync(RentalApplicationUpdateRequest rentalApplication);
        Task<ResponseDTO> DeleteRentalApplicationAsync(RentalApplicationDeleteRequest rentalApplication);
        Task<ResponseDTO<PagedList<RentalApplicationSearchDTO>>> SearchRentalApplicationAsync(RentalApplicationSearchRequest search);
        Task<ResponseDTO<RentalApplicationRegisterRequest>> GetByIdAsync(int? id);
        Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, RentalApplicationSearchRequest search);
    }
}
