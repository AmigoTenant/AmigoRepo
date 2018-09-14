using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class LocationTypeMap : EntityTypeConfiguration<LocationType>
    {
        public LocationTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.LocationTypeId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LocationType");
            this.Property(t => t.LocationTypeId).HasColumnName("LocationTypeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
