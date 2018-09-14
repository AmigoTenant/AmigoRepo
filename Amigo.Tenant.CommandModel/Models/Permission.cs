using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Permission : ValidatableBase
    {
        public int PermissionId { get; set; }
        public int? AmigoTenantTRoleId { get; set; }
        public int? ActionId { get; set; }
        public virtual Action Action { get; set; }
        public virtual AmigoTenantTRole AmigoTenantTRole { get; set; }
    }
}