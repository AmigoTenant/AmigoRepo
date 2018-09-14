using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(255);

            this.Property(t => t.ShortName)
                .HasMaxLength(50);

            this.Property(t => t.IsHazardous)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Product");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.IsHazardous).HasColumnName("IsHazardous");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
