using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class LocationCoordinateMap : EntityTypeConfiguration<LocationCoordinate>
    {
        public LocationCoordinateMap()
        {
            // Primary Key
            this.HasKey(t => t.LocationCoordinateId);

            // Properties
            // Table & Column Mappings
            this.ToTable("LocationCoordinate");
            this.Property(t => t.LocationCoordinateId).HasColumnName("LocationCoordinateId");
            this.Property(t => t.Latitude).HasColumnName("Latitude").HasPrecision(16,9);
            this.Property(t => t.Longitude).HasColumnName("Longitude").HasPrecision(16, 9);
            this.Property(t => t.LocationId).HasColumnName("LocationId");


            // Relationships
            this.HasOptional(t => t.Location)
                .WithMany(t => t.LocationCoordinates)
                .HasForeignKey(d => d.LocationId);

        }
    }
}
