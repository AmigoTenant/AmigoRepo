using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class LocationCoordinateMapping : Map<LocationCoordinateDTO>
    {
        public LocationCoordinateMapping()
        {
            TableName("vwLocationCoordinate");

            Columns(x =>
            {
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.LocationCode);
            });

        }
    }
}
