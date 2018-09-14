using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AuditLogMap : EntityTypeConfiguration<AuditLog>
    {
        public AuditLogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Command)
                .HasMaxLength(1000);

            this.Property(t => t.PostTime)
                .HasMaxLength(24);

            this.Property(t => t.HostName)
                .HasMaxLength(100);

            this.Property(t => t.LoginName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("AuditLog");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Command).HasColumnName("Command");
            this.Property(t => t.PostTime).HasColumnName("PostTime");
            this.Property(t => t.HostName).HasColumnName("HostName");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
        }
    }
}
