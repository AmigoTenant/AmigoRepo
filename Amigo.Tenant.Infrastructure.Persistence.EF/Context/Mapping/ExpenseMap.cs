using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ExpenseMap : EntityTypeConfiguration<Expense>
    {
        public ExpenseMap()
        {
            // Primary Key
            this.HasKey(t => t.ExpenseId);
            // Table & Column Mappings
            this.ToTable("Expense");
            this.Property(t=> t.ExpenseId).HasColumnName("ExpenseId");
            this.Property(t=> t.ExpenseDate).HasColumnName("ExpenseDate");
            this.Property(t=> t.PaymentTypeId).HasColumnName("PaymentTypeId");
            this.Property(t=> t.HouseId).HasColumnName("HouseId");
            this.Property(t=> t.PeriodId).HasColumnName("PeriodId");
            this.Property(t=> t.ReferenceNo).HasColumnName("ReferenceNo");
            this.Property(t=> t.Remark).HasColumnName("Remark");
            this.Property(t=> t.SubTotalAmount).HasColumnName("SubTotalAmount");
            this.Property(t=> t.Tax).HasColumnName("Tax");
            this.Property(t=> t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t=> t.RowStatus          ).HasColumnName("RowStatus");
            this.Property(t=> t.CreatedBy          ).HasColumnName("CreatedBy");
            this.Property(t=> t.CreationDate       ).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy         ).HasColumnName("UpdatedBy");

    }
}
}
