using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class DriverReportMap : EntityTypeConfiguration<DriverReport>
    {
        public DriverReportMap()
        {
            // Primary Key
            this.HasKey(t => t.DriverReportId);

            // Properties
            this.Property(t => t.ApproverSignature)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DriverReport");
            this.Property(t => t.DriverReportId).HasColumnName("DriverReportId");
            this.Property(t => t.ReportDate).HasColumnName("ReportDate");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.WeekNumber).HasColumnName("WeekNumber");
            this.Property(t => t.DriverUserId).HasColumnName("DriverUserId");
            this.Property(t => t.ApproverUserId).HasColumnName("ApproverUserId");
            this.Property(t => t.ApproverSignature).HasColumnName("ApproverSignature");
            this.Property(t => t.DayPayDriverTotal).HasColumnName("DayPayDriverTotal");
            this.Property(t => t.DayBillCustomerTotal).HasColumnName("DayBillCustomerTotal");
            this.Property(t => t.TotalHours).HasColumnName("TotalHours");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.FinishTime).HasColumnName("FinishTime");

            // Relationships
            this.HasOptional(t => t.AmigoTenantTUser)
                .WithMany(t => t.DriverReports)
                .HasForeignKey(d => d.ApproverUserId);
            this.HasOptional(t => t.AmigoTenantTUser1)
                .WithMany(t => t.DriverReports1)
                .HasForeignKey(d => d.DriverUserId);

        }
    }
}
