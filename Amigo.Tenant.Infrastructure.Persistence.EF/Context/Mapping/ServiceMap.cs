using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ServiceMap : EntityTypeConfiguration<Service>
    {
        public ServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.IsPerMove)
                .HasMaxLength(1);

            this.Property(t => t.IsPerHour)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Service");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsPerMove).HasColumnName("IsPerMove");
            this.Property(t => t.IsPerHour).HasColumnName("IsPerHour");
            this.Property(t => t.ServiceTypeId).HasColumnName("ServiceTypeId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.ServiceType)
                .WithMany(t => t.Services)
                .HasForeignKey(d => d.ServiceTypeId);

        }
    }
}
