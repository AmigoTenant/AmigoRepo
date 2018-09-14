using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.Application.Services.Interfaces.Security
{
    public interface IAmigoTenantTUserApplicationService
    {
        ISClientSettings IdentityServerClientSettings
        {
            get; set;
        }
        Task<ResponseDTO<PagedList<AmigoTenantTUserDTO>>> SearchUsersByCriteriaAsync(UserSearchRequest search);
        Task<AmigoTenantTUserDTO> SearchUsersByIdAsync(UserSearchRequest search);
        Task<bool> Exists(UserSearchRequest search);
        Task<ResponseDTO> Register(AmigoTenantTUserDTO dto);
        Task<ResponseDTO> Update(AmigoTenantTUserDTO dto);
        Task<ResponseDTO> Delete(AmigoTenantTUserStatusDTO dto);
        Task<ResponseDTO> ValidateAuthorizationAsync(AuthorizationRequest search, int? amigoTenantTUserId);
        Task<ResponseDTO<List<AmigoTenantTUserBasicDTO>>> SearchUsersByCriteriaBasicAsync(UserSearchBasicRequest search);
        Task<AmigoTenantTUserBasicDTO> GetBaseUserInfoById(int amigoTenantTUserId);
        Task<UserResponse> ValidateUserName(UserSearchRequest search);
        Task<AmigoTenantTUserAuditDTO> SearchByIdForAudit(int? createdBy, int? updatedBy);
        Task<bool> IsAdmin(int amigoTenantTUserId);

    }
}
