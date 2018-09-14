using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.IdentityServer.DTOs.Requests.Users;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Common;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Users;

namespace Amigo.Tenant.IdentityServer.ApplicationServices.Services
{
    public interface IUserAdministrationService
    {
        Task<ResponseDTO<UserResponse>> GetDetails(string username);
        Task<ResponseDTO<List<UserResponse>>> GetUsersDetails(List<string> usernames);
        Task<ResponseDTO<PagedList<UserResponse>>> Search(SearchUserRequest search);
        Task<ResponseDTO<UserResponse>> Register(RegisterUserRequest user);
        Task<ResponseDTO<UserResponse>> RegisterExternalUser(RegisterExternalUserRequest user);
        Task<ResponseDTO> Update(UpdateUserRequest user);
        Task<ResponseDTO> AddClaim(AddClaimRequest claim);
        Task<ResponseDTO> RemoveClaim(RemoveClaimRequest claim);
        Task<ResponseDTO> ResetPassword(ResetUserPasswordRequest resetPasswordRequest);
        Task<ResponseDTO> ChangePassword(ChangeUserPasswordRequest changePasswordRequest);
        Task<ResponseDTO> ChangeStatus(UpdateStatusUserRequest user);
    }
}