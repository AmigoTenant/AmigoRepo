using ExpressMapper;
using System.Data.Entity.Spatial;
using System.Globalization;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{

    public class LocationProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterLocationCommand, Location>()
                .Function(dest => dest.Coordinate, source => DbGeometry.FromText(string.Format("POINT({0} {1})", source.Longitude.ToString(CultureInfo.InvariantCulture), source.Latitude.ToString(CultureInfo.InvariantCulture)) ,DbGeography.DefaultCoordinateSystemId));

            Mapper.Register<UpdateLocationCommand, Location>()
                .Function(dest => dest.Coordinate, source => DbGeometry.FromText(string.Format("POINT({0} {1})", source.Longitude.ToString(CultureInfo.InvariantCulture), source.Latitude.ToString(CultureInfo.InvariantCulture)), DbGeography.DefaultCoordinateSystemId));

            Mapper.Register<DeleteLocationCommand, Location>();
        }
    }


}
