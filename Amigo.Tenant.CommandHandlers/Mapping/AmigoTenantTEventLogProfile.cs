using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;

using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class AmigoTenantTEventLogProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterAmigoTenanttEventLogCommand, AmigoTenantTEventLog>();
            
        }
    }
}
