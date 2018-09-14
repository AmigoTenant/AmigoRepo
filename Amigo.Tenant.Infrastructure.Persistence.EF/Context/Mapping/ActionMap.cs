using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ActionMap : EntityTypeConfiguration<Action>
    {
        public ActionMap()
        {
            // Primary Key
            this.HasKey(t => t.ActionId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Code)
                .IsRequired();

            this.Property(t => t.Description)
                .HasMaxLength(200);

            this.Property(t => t.Type)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Action");
            this.Property(t => t.ActionId).HasColumnName("ActionId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.Module)
                .WithMany(t => t.Actions)
                .HasForeignKey(d => d.ModuleId);

        }
    }
}
