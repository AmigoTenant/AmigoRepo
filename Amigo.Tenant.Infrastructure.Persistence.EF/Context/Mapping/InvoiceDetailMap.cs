using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class InvoiceDetailMap : EntityTypeConfiguration<InvoiceDetail>
    {
        public InvoiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.InvoiceDetailId);

            // Table & Column Mappings
            this.ToTable("InvoiceDetail");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.PaymentPeriodId).HasColumnName("PaymentPeriodId");
            this.Property(t => t.ConceptId).HasColumnName("ConceptId");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
        }
    }
}
