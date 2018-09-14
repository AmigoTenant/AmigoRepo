using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class InvoiceMap : EntityTypeConfiguration<Invoice>
    {
        public InvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.InvoiceId);

            // Table & Column Mappings
            this.ToTable("Invoice");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.PaymentTypeId).HasColumnName("PaymentTypeId");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.Taxes).HasColumnName("Taxes");
            this.Property(t => t.BusinessPartnerId).HasColumnName("BusinessPartnerId");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.PaymentOperationNo).HasColumnName("PaymentOperationNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.PaymentOperationDate).HasColumnName("PaymentOperationDate");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.InvoiceStatusId).HasColumnName("InvoiceStatusId");
            this.Property(t => t.TotalRent).HasColumnName("TotalRent");
            this.Property(t => t.TotalDeposit).HasColumnName("TotalDeposit");
            this.Property(t => t.TotalLateFee).HasColumnName("TotalLateFee");
            this.Property(t => t.TotalService).HasColumnName("TotalService");
            this.Property(t => t.TotalFine).HasColumnName("TotalFine");
            this.Property(t => t.TotalOnAcount).HasColumnName("TotalOnAcount");
        }
    }
}









