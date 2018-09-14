using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class AmigoTenantTRoleProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<AmigoTenantTRoleDTO, RegisterAmigoTenantTRoleCommand>();
            Mapper.Register<AmigoTenantTRoleStatusDTO, DeleteAmigoTenantTRoleCommand>();

            Mapper.Register<ActionDTO, ModuleTreeDTO>()
                .Value(x=>x.ModuleTreeType,ModuleTreeType.Action)
                .Member(x => x.ParentCode, y => y.ModuleCode)                
                .Ignore(x=>x.Children);

            Mapper.Register<ModuleDTO, ModuleTreeDTO>()
                .Value(x => x.ModuleTreeType, ModuleTreeType.Module)
                .Member(x => x.ParentCode, y => y.ParentModuleCode)                
                .Ignore(x => x.Children);
        }
    }
}
