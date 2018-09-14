using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using System.IO;
using System.Net.Http;
using System.Net;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IContractApplicationService
    {
        Task<ResponseDTO> RegisterContractAsync(ContractRegisterRequest contract);
        Task<ResponseDTO> UpdateContractAsync(ContractUpdateRequest contract);
        Task<ResponseDTO> DeleteContractAsync(ContractDeleteRequest contract);
        Task<ResponseDTO<PagedList<ContractSearchDTO>>> SearchContractAsync(ContractSearchRequest search);
        Task<ResponseDTO<ContractRegisterRequest>> GetByIdAsync(int? id);
        //Task<ResponseDTO<<ContractTypeAheadDTO>>> SearchContractAllTypeAhead(string name);
        Task<ResponseDTO<List<HouseFeatureDetailContractDTO>>> SearchHouseFeatureDetailContractAsync(int? houseId);
        Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ContractSearchRequest search);
        Task<ResponseDTO> ChangeStatusContractAsync(ContractChangeStatusRequest contractId);
    }
}
