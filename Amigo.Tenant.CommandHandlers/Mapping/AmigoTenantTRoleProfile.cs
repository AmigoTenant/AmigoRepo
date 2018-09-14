using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class AmigoTenantTRoleProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterAmigoTenantTRoleCommand, AmigoTenantTRole>();
            Mapper.Register<UpdateAmigoTenantTRoleCommand, AmigoTenantTRole>();
            Mapper.Register<DeleteAmigoTenantTRoleCommand, AmigoTenantTRole>();
        }
    }
}
