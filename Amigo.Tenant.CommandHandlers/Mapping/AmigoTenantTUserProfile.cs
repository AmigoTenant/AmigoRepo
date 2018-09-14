using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Security.Authorization;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class AmigoTenantTUserProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterAmigoTenantTUserCommand, AmigoTenantTUser>()
            .Member(x => x.Username, y => y.Username != null ? y.Username.ToUpper() : null);


            Mapper.Register<UpdateAmigoTenantTUserCommand, AmigoTenantTUser>();
            Mapper.Register<DeleteAmigoTenantTUserCommand, AmigoTenantTUser>();
            //Mapper.Register<UpdateDeviceAuthorizationCommand, AmigoTenantTUser>();
            Mapper.Register<UserAuthorizationCommand, AmigoTenantTUser>();
            
            //Mapper.Register<UpdateDeviceAuthorizationCommand, Device>();
        }
    }
}
