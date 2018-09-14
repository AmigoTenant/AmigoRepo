namespace Amigo.Tenant.IdentityServer.DTOs.Responses.Users
{
    public class UserResponse
    {
        public UserResponse()
        {
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Email { get; set; }        
        public string PhoneNumber { get; set; }
        public bool? RowStatus { get; set; }
        public UserClaim[] Claims { get; set; }
    }

    public class UserClaim
    {
        public virtual string ClaimType { get; set; }

        /// <summary>Claim value</summary>
        public virtual string ClaimValue { get; set; }
    }
}
