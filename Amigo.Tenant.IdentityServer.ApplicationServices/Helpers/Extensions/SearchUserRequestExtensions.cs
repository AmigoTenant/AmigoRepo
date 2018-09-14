using Amigo.Tenant.IdentityServer.ApplicationServices.Extensions;
using Amigo.Tenant.IdentityServer.DTOs.Requests.Users;

namespace Amigo.Tenant.IdentityServer.ApplicationServices.Helpers.Extensions
{
    public static class SearchUserRequestExtensions
    {
        public static bool IsSearchingByUserNameOnly(this SearchUserRequest search)
        {
            return
                search != null &&
                search.UserName.IsNotEmpty() &&
                search.Email.IsEmpty() &&
                search.FirstName.IsEmpty() &&
                search.LastName.IsEmpty() &&
                search.Claim.IsEmpty();
        }
    }
}