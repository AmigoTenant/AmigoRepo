namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class UserSearchBasicRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomUsername { get; set; }

    }
}
