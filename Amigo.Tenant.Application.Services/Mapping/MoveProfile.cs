using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class MoveProfile: Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterMoveRequest,RegisterMoveCommand>();
        }
    }
}
