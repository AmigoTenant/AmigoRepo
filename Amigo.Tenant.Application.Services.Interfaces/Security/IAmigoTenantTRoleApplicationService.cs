using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;


namespace Amigo.Tenant.Application.Services.Interfaces.Security
{
    public interface IAmigoTenantTRoleApplicationService
    {
        Task<ResponseDTO<PagedList<AmigoTenantTRoleDTO>>> SearchAmigoTenantTRoleByCriteriaAsync(AmigoTenantTRoleSearchRequest search);
        //Task<AmigoTenantTRoleDTO> SearchRoleByIdAsync(AmigoTenantTRoleSearchRequest search);
        Task<bool> Exists(AmigoTenantTRoleSearchRequest search);
        Task<ResponseDTO> Register(AmigoTenantTRoleDTO dto);
        Task<ResponseDTO> Update(AmigoTenantTRoleDTO dto);

        Task<ResponseDTO> UpdateActions(AmigoTenanttRolPermissionRequest actions);

        Task<ResponseDTO> Delete(AmigoTenantTRoleStatusDTO dto);

        Task<ResponseDTO<PagedList<AmigoTenantTRoleBasicDTO>>> SearchAmigoTenantTRoleBasicCriteriaAsync(
            AmigoTenantTRoleSearchRequest search);

        Task<ResponseDTO<AmigoTenantTRoleBasicDTO>> GetAmigoTenantTRoleBasicByIdAsync(AmigoTenantTRoleSearchRequest search);

        Task<ResponseDTO<List<ModuleTreeDTO>>> GetRoleTree(string roleCode);
        Task<ResponseDTO<List<ModuleTreeDTO>>> GetModuleActionAsyn();

    }
}
