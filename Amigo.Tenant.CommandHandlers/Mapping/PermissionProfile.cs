using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class PermissionProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterPermissionCommand, Permission>();
            Mapper.Register<UpdatePermissionCommand, Permission>();
            Mapper.Register<DeletePermissionCommand, Permission>();
        }
    }
}
