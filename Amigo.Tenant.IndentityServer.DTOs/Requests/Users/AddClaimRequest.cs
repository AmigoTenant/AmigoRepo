namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Users
{
    public abstract class ClaimRequestBase
    {
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }        
    }

    public class AddClaimRequest: ClaimRequestBase
    {        
    }

    public class RemoveClaimRequest : ClaimRequestBase
    {
    }
}