using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Security 
{
    public interface IPermissionApplicationService
    {
        Task<ResponseDTO<PagedList<PermissionDTO>>> SearchPermissionByCriteriaAsync(PermissionSearchRequest search);
        //Task<PermissionDTO> SearchPermissionByIdAsync(PermissionSearchRequest search);
        Task<bool> Exists(PermissionSearchRequest search);
        Task<ResponseDTO> Register(PermissionDTO dto);
        //Task<ResponseDTO> Update(PermissionDTO dto);
        Task<ResponseDTO> Delete(PermissionStatusDTO dto);
    }
}
