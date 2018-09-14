using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ExpenseDetailMap : EntityTypeConfiguration<ExpenseDetail>
    {
        public ExpenseDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.ExpenseDetailId);
            // Table & Column Mappings
            this.ToTable("ExpenseDetail");
            this.Property(t=> t.ExpenseId).HasColumnName("ExpenseId");
            this.Property(t => t.ExpenseDetailId).HasColumnName("ExpenseDetailId");
            this.Property(t=> t.ConceptId).HasColumnName("ConceptId");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Tax).HasColumnName("Tax");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t=> t.TenantId).HasColumnName("TenantId");
            this.Property(t=> t.ExpenseDetailStatusId).HasColumnName("ExpenseDetailStatusId");
            this.Property(t=> t.Quantity).HasColumnName("Quantity");
            this.Property(t=> t.RowStatus          ).HasColumnName("RowStatus");
            this.Property(t=> t.CreatedBy          ).HasColumnName("CreatedBy");
            this.Property(t=> t.CreationDate       ).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy         ).HasColumnName("UpdatedBy");

    }
}
}
