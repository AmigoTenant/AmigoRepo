using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class BrandMap : EntityTypeConfiguration<Brand>
    {
        public BrandMap()
        {
            // Primary Key
            this.HasKey(t => t.BrandId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Brand");
            this.Property(t => t.BrandId).HasColumnName("BrandId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
