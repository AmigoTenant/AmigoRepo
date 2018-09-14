using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AmigoTenantTServiceChargeMap : EntityTypeConfiguration<AmigoTenantTServiceCharge>
    {
        public AmigoTenantTServiceChargeMap()
        {
            // Primary Key
            this.HasKey(t => t.AmigoTenantTServiceChargeId);

            // Properties
            // Table & Column Mappings
            this.ToTable("AmigoTenantTServiceCharge");
            this.Property(t => t.AmigoTenantTServiceChargeId).HasColumnName("AmigoTenantTServiceChargeId");
            this.Property(t => t.DriverPay).HasColumnName("DriverPay");
            this.Property(t => t.CustomerBill).HasColumnName("CustomerBill");
            this.Property(t => t.AmigoTenantTServiceId).HasColumnName("AmigoTenantTServiceId");
            this.Property(t => t.RateId).HasColumnName("RateId");
            this.Property(t => t.DriverReportId).HasColumnName("DriverReportId");

            // Relationships
            this.HasOptional(t => t.DriverReport)
                .WithMany(t => t.AmigoTenantTServiceCharges)
                .HasForeignKey(d => d.DriverReportId);
            this.HasOptional(t => t.Rate)
                .WithMany(t => t.AmigoTenantTServiceCharges)
                .HasForeignKey(d => d.RateId);
            this.HasOptional(t => t.AmigoTenantTService)
                .WithMany(t => t.AmigoTenantTServiceCharges)
                .HasForeignKey(d => d.AmigoTenantTServiceId);

        }
    }
}
