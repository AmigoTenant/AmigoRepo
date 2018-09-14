using ExpressMapper;
using System.Data.Entity.Spatial;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{

    public class DeviceProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterDeviceCommand, Device>();
            Mapper.Register<UpdateDeviceCommand, Device>();
            Mapper.Register<DeleteDeviceCommand, Device>();
        }
    }


}
