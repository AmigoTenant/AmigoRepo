namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class UserSearchRequest
    {
        public int AmigoTenantTUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DedicatedLocationId { get; set; }
        public string UserType { get; set; }
        public int? AmigoTenantTRoleId { get; set; }
        public string PayBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
