using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class DeviceMap : EntityTypeConfiguration<Device>
    {
        public DeviceMap()
        {
            // Primary Key
            this.HasKey(t => t.DeviceId);

            // Properties
            this.Property(t => t.Identifier)
                .HasMaxLength(100);

            this.Property(t => t.WIFIMAC)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Device");
            this.Property(t => t.DeviceId).HasColumnName("DeviceId");
            this.Property(t => t.Identifier).HasColumnName("Identifier");
            this.Property(t => t.WIFIMAC).HasColumnName("WIFIMAC");
            this.Property(t => t.OSVersionId).HasColumnName("OSVersionId");
            this.Property(t => t.ModelId).HasColumnName("ModelId");
            this.Property(t => t.IsAutoDateTime).HasColumnName("IsAutoDateTime");
            this.Property(t => t.IsSpoofingGPS).HasColumnName("IsSpoofingGPS");
            this.Property(t => t.IsRootedJailbreaked).HasColumnName("IsRootedJailbreaked");
            this.Property(t => t.AppVersionId).HasColumnName("AppVersionId");
            this.Property(t => t.AssignedAmigoTenantTUserId).HasColumnName("AssignedAmigoTenantTUserId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.AppVersion)
                .WithMany(t => t.Devices)
                .HasForeignKey(d => d.AppVersionId);
            this.HasOptional(t => t.Model)
                .WithMany(t => t.Devices)
                .HasForeignKey(d => d.ModelId);
            this.HasOptional(t => t.OSVersion)
                .WithMany(t => t.Devices)
                .HasForeignKey(d => d.OSVersionId);
            this.HasOptional(t => t.AmigoTenantTUser)
                .WithMany(t => t.Devices)
                .HasForeignKey(d => d.AssignedAmigoTenantTUserId);

        }
    }
}
