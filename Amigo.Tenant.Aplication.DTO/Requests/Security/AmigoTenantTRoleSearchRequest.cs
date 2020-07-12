namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class AmigoTenantTRoleSearchRequest
    {
        public int? AmigoTenantTRoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsAdmin { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
