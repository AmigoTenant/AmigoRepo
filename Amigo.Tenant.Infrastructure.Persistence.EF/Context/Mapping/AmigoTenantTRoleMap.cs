using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AmigoTenantTRoleMap : EntityTypeConfiguration<AmigoTenantTRole>
    {
        public AmigoTenantTRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.AmigoTenantTRoleId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            //this.Property(t => t.IsAdmin)
            //    .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("AmigoTenantTRole");
            this.Property(t => t.AmigoTenantTRoleId).HasColumnName("AmigoTenantTRoleId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsAdmin).HasColumnName("IsAdmin");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
