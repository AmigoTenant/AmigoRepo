using ExpressMapper;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class DeviceProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterDeviceRequest, RegisterDeviceCommand>();
            Mapper.Register<UpdateDeviceRequest, UpdateDeviceCommand>();
            Mapper.Register<DeleteDeviceRequest, DeleteDeviceCommand>();
        }
    }
}
