namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class PermissionSearchRequest
    {
        public int PermissionId { get; set; }
        public string ActionCode { get; set; }
        public string AmigoTenantTRoleCode { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
