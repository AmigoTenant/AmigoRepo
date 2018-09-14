using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
namespace Amigo.Tenant.Application.Services.Mapping
{
    public class AmigoTenanttServiceProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<AmigoTenanttServiceDTO, RegisterAmigoTenanttServiceCommand>();
            Mapper.Register<AmigoTenantTServiceRequest, RegisterAmigoTenanttServiceCommand>();

            Mapper.Register<DriverReportDTO, RegisterDriverReportCommand>();

        }
    }
}
