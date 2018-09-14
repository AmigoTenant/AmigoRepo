using Amigo.Tenant.Application.Services.Mapping;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            //TODO:Register all mapping profiles
            
            #region DTO to Command Mappings

            MapperConfig.Register<MoveProfile>();
            MapperConfig.Register<AmigoTenantTUserProfile>();
            MapperConfig.Register<LocationProfile>();
            MapperConfig.Register<LocationCoordinateProfile>();
            MapperConfig.Register<AmigoTenanttServiceProfile>();
            MapperConfig.Register<DeviceProfile>();
            MapperConfig.Register<AmigoTenantTRoleProfile>();
            MapperConfig.Register<MainTenantProfile>();
            MapperConfig.Register<HouseProfile>();

            #endregion


            #region Command to Models Mappings

            MapperConfig.Register<CommandHandlers.Mapping.MoveProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.AmigoTenantTUserProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.LocationProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.LocationCoordinateProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.AmigoTenanttServiceProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.DeviceProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.PermissionProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.AmigoTenantTEventLogProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.AmigoTenantTRoleProfile>();
            MapperConfig.Register<CommandHandlers.Mapping.MainTenantProfile>();

            #endregion


            #region Querys to DTOS

            #endregion

            //Init
            MapperConfig.Init();
        }
    }
}