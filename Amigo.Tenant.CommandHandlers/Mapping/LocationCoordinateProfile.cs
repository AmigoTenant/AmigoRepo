using ExpressMapper;
using System.Data.Entity.Spatial;
using System.Globalization;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{

    public class LocationCoordinateProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterLocationCoordinateItem, LocationCoordinate>()
                .Function(dest => dest.Coordinate, source => DbGeometry.FromText(string.Format("POINT({0} {1})", source.Longitude.ToString(CultureInfo.InvariantCulture), source.Latitude.ToString(CultureInfo.InvariantCulture)), DbGeography.DefaultCoordinateSystemId));

            Mapper.Register<DeleteLocationCoordinatesCommand, LocationCoordinate>();

        }
    }

}
