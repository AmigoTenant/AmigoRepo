using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AppVersionMap : EntityTypeConfiguration<AppVersion>
    {
        public AppVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.AppVersionId);

            // Properties
            this.Property(t => t.Version)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ReleaseNotes)
                .HasMaxLength(4096);

            // Table & Column Mappings
            this.ToTable("AppVersion");
            this.Property(t => t.AppVersionId).HasColumnName("AppVersionId");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ReleaseDate).HasColumnName("ReleaseDate");
            this.Property(t => t.ReleaseNotes).HasColumnName("ReleaseNotes");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
