using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;


namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface ICostCenterApplicationService
    {
        Task<ResponseDTO<PagedList<CostCenterDTO>>> SearchCostCenterByNameAsync(CostCenterSearchRequest search);
        Task<ResponseDTO<List<CostCenterDTO>>> SearchCostCenterAll();
        Task<ResponseDTO<List<CostCenterTypeAheadDTO>>> SearchCostCenterAllTypeAhead(string name);
        Task<CostCenterTypeAheadDTO> SearchCostCenterByNameAsync(string name);
        Task<ResponseDTO> RegisterModuleAsync(CostCenterDTO costCenter);
        Task<ResponseDTO> UpdateCostCenterAsync(CostCenterDTO costCenter);
        Task<ResponseDTO> DeleteCostCenterAsync(CostCenterDTO costCenter);
    }
}
