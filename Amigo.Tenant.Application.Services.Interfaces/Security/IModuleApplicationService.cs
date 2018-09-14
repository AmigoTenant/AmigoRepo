using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Application.Services.Interfaces.Security
{
    public interface IModuleApplicationService
    {
        Task<ResponseDTO<PagedList<ModuleDTO>>> SearchModulesAsync(ModuleSearchRequest search);
        Task<ResponseDTO<ModuleActionsDTO>> GetModuleAsync(GetModuleRequest getRequest);
        Task<ResponseDTO> RegisterModuleAsync(RegisterModuleRequest newModule);
        Task<ResponseDTO> UpdateModuleAsync(UpdateModuleRequest module);
        Task<ResponseDTO> DeleteModuleAsync(DeleteModuleRequest module);
    }
}
