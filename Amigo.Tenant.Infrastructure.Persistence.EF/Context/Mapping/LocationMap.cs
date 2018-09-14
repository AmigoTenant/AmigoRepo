using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap()
        {
            // Primary Key
            this.HasKey(t => t.LocationId);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Address1)
                .HasMaxLength(100);

            this.Property(t => t.Address2)
                .HasMaxLength(100);

            this.Property(t => t.ZipCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Location");
            this.Property(t => t.LocationId).HasColumnName("LocationId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LocationTypeId).HasColumnName("LocationTypeId");
            this.Property(t => t.ParentLocationId).HasColumnName("ParentLocationId");
            this.Property(t => t.Latitude).HasColumnName("Latitude").HasPrecision(16, 9);
            this.Property(t => t.Longitude).HasColumnName("Longitude").HasPrecision(16, 9);
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.HasGeofence).HasColumnName("HasGeofence");

            // Relationships
            this.HasOptional(t => t.City)
                .WithMany(t => t.Locations)
                .HasForeignKey(d => d.CityId);
            this.HasOptional(t => t.LocationType)
                .WithMany(t => t.Locations)
                .HasForeignKey(d => d.LocationTypeId);

        }
    }
}
