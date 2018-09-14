using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            // Primary Key
            this.HasKey(t => t.ModuleId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

               

            this.Property(t => t.URL)
                .HasMaxLength(200);


            this.Property(t => t.Name)
                .IsRequired();


            this.Property(t => t.Code)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Module");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.ParentModuleId).HasColumnName("ParentModuleId");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
