using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class OSVersionMap : EntityTypeConfiguration<OSVersion>
    {
        public OSVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.OSVersionId);

            // Properties
            this.Property(t => t.Version)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OSVersion");
            this.Property(t => t.OSVersionId).HasColumnName("OSVersionId");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PlatformId).HasColumnName("PlatformId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.Platform)
                .WithMany(t => t.OSVersions)
                .HasForeignKey(d => d.PlatformId);

        }
    }
}
