using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class PaymentPeriodMap : EntityTypeConfiguration<PaymentPeriod>
    {
        public PaymentPeriodMap()
        {
            // Primary Key
            this.HasKey(t => t.PaymentPeriodId);

            // Table & Column Mappings
            this.ToTable("PaymentPeriod");
            this.Property(t => t.ConceptId).HasColumnName("ConceptId");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.TenantId).HasColumnName("TenantId");
            this.Property(t => t.PeriodId).HasColumnName("PeriodId");
            this.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.PaymentPeriodStatusId).HasColumnName("PaymentPeriodStatusId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.PaymentTypeId).HasColumnName("PaymentTypeId");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.ReferenceNo).HasColumnName("ReferenceNo");
            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
        }
    }
}

