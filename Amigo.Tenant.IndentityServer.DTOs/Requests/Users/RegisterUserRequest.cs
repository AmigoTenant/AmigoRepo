namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Users
{
    public class RegisterUserRequest:RegisterExternalUserRequest
    {        
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }        
    }
}
