using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class RateMap : EntityTypeConfiguration<Rate>
    {
        public RateMap()
        {
            // Primary Key
            this.HasKey(t => t.RateId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            this.Property(t => t.PaidBy)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Rate");
            this.Property(t => t.RateId).HasColumnName("RateId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PaidBy).HasColumnName("PaidBy");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.BillCustomer).HasColumnName("BillCustomer");
            this.Property(t => t.PayDriver).HasColumnName("PayDriver");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.Service)
                .WithMany(t => t.Rates)
                .HasForeignKey(d => d.ServiceId);

        }
    }
}
