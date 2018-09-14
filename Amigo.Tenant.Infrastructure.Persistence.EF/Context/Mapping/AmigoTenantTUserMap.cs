using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AmigoTenantTUserMap : EntityTypeConfiguration<AmigoTenantTUser>
    {
        public AmigoTenantTUserMap()
        {
            // Primary Key
            this.HasKey(t => t.AmigoTenantTUserId);

            // Properties
            this.Property(t => t.AmigoTenantTUserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Username)
                .HasMaxLength(64);

            this.Property(t => t.FirstName)
                 .HasMaxLength(50);

            this.Property(t => t.LastName)
                 .HasMaxLength(50);

            this.Property(t => t.PayBy)
                .HasMaxLength(1);

            this.Property(t => t.UserType)
                .HasMaxLength(1);

            //this.Property(t => t.BypassDeviceValidation)
            //    .HasMaxLength(1);

            this.Property(t => t.UnitNumber)
                .HasMaxLength(20);

            this.Property(t => t.TractorNumber)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("AmigoTenantTUser");
            this.Property(t => t.AmigoTenantTUserId).HasColumnName("AmigoTenantTUserId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.AmigoTenantTRoleId).HasColumnName("AmigoTenantTRoleId");
            this.Property(t => t.PayBy).HasColumnName("PayBy");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.DedicatedLocationId).HasColumnName("DedicatedLocationId");
            this.Property(t => t.BypassDeviceValidation).HasColumnName("BypassDeviceValidation");
            this.Property(t => t.UnitNumber).HasColumnName("UnitNumber");
            this.Property(t => t.TractorNumber).HasColumnName("TractorNumber");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.Location)
                .WithMany(t => t.AmigoTenantTUsers)
                .HasForeignKey(d => d.DedicatedLocationId);
            this.HasOptional(t => t.AmigoTenantTRole)
                .WithMany(t => t.AmigoTenantTUsers)
                .HasForeignKey(d => d.AmigoTenantTRoleId);

        }
    }
}
