namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Users
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? RowStatus { get; set; }
    }

    public class ResetUserPasswordRequest
    {
        public int Id { get; set; }        
        public string Password { get; set; }
    }

    public class ChangeUserPasswordRequest
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}