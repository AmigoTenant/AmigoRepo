using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{

    public class LocationCoordinateProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterLocationCoordinatesRequest, RegisterLocationCoordinatesCommand>();
            Mapper.Register<DeleteLocationCoordinatesRequest, DeleteLocationCoordinatesCommand>();
        }
    }
}
