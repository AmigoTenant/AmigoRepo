using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Commands.Security.Permission;

using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class PermissionProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<PermissionDTO, RegisterPermissionCommand>();

            Mapper.Register<PermissionDTO, DeletePermissionCommand>();

            Mapper.Register<PermissionStatusDTO, DeletePermissionCommand>();

            Mapper.Register<AmigoTenanttRolPermissionRequest, DeletePermissionCommand>();

            Mapper.Register<AmigoTenanttRolPermissionRequest, RegisterPermissionCommand>();
        }
    }
}
