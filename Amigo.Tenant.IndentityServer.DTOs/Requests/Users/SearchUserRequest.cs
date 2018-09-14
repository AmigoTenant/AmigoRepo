using Amigo.Tenant.IdentityServer.DTOs.Requests.Common;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Users;

namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Users
{
    public class SearchUserRequest: PagedRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Email { get; set; }        
        public UserClaim Claim { get; set; }        
    }
}
