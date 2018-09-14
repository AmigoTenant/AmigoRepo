using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.PermissionId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Permission");
            this.Property(t => t.PermissionId).HasColumnName("PermissionId");
            this.Property(t => t.AmigoTenantTRoleId).HasColumnName("AmigoTenantTRoleId");
            this.Property(t => t.ActionId).HasColumnName("ActionId");

            // Relationships
            this.HasOptional(t => t.Action)
                .WithMany(t => t.Permissions)
                .HasForeignKey(d => d.ActionId);
            this.HasOptional(t => t.AmigoTenantTRole)
                .WithMany(t => t.Permissions)
                .HasForeignKey(d => d.AmigoTenantTRoleId);

        }
    }
}
