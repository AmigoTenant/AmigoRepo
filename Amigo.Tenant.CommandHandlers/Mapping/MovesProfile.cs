using ExpressMapper;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class MoveProfile:Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterMoveCommand, Move>();
        }
    }
}
