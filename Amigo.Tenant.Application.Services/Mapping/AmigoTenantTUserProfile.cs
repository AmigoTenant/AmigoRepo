using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Commands.Security.Authorization;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class AmigoTenantTUserProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<AmigoTenantTUserDTO,RegisterAmigoTenantTUserCommand>();
            Mapper.Register<AmigoTenantTUserStatusDTO, DeleteAmigoTenantTUserCommand>();
            Mapper.Register<AuthorizationRequest, UpdateDeviceAuthorizationCommand>();
        }
    }
}
