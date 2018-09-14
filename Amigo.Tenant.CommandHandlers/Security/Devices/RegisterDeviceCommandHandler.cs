using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;

namespace Amigo.Tenant.CommandHandlers.Security.Devices
{
    public class RegisterDeviceCommandHandler : IAsyncCommandHandler<RegisterDeviceCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Device> _deviceRepository;

        public RegisterDeviceCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Device> deviceRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterDeviceCommand message)
        {
            //Validate using domain models
            var device = _mapper.Map<RegisterDeviceCommand, Device>(message);

            device.RowStatus = true;
            device.Creation(message.UserId);

            //if is not valid
            if (device.HasErrors) return device.ToResult();

            //Insert
            _deviceRepository.Add(device);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new DeviceRegistered() {  DeviceId = device.DeviceId});

            //Return result
            return device.ToResult();
        }
    }
}
