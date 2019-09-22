using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ContractChangeStatusMap : EntityTypeConfiguration<ContractChangeStatus>
    {
        public ContractChangeStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractChangeStatusId);
            this.ToTable("ContractChangeStatus");
            this.Property(t=> t.ContractChangeStatusId).HasColumnName("ContractChangeStatusId");
            this.Property(t=> t.ContractStatusId).HasColumnName("ContractStatusId");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t=> t.TenantId).HasColumnName("TenantId");
            this.Property(t=> t.HouseId).HasColumnName("HouseId");
            this.Property(t=> t.Rent).HasColumnName("Rent");
            this.Property(t=> t.Deposit).HasColumnName("Deposit");
            this.Property(t=> t.BeginPeriodId).HasColumnName("BeginPeriodId");
            this.Property(t=> t.EndPeriodId).HasColumnName("EndPeriodId");
            this.Property(t => t.ContractTermType).HasColumnName("ContractTermType");
            this.Property(t=> t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t=> t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

        }
    }
}
