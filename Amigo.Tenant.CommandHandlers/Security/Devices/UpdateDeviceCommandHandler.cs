using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.Security.Devices
{

    public class UpdateDeviceCommandHandler : IAsyncCommandHandler<UpdateDeviceCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Device> _deviceRepository;

        public UpdateDeviceCommandHandler(
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

        public async Task<CommandResult> Handle(UpdateDeviceCommand message)
        {
            //Validate using domain models
            Device deviceAux = new Device();
            deviceAux = _mapper.Map<UpdateDeviceCommand, Device>(message);

            //validate code uniqueness
            var existingDevice = await _deviceRepository.GetDevice(message.DeviceId);

            if (existingDevice == null)
            {
                deviceAux.AddError("Device Id not found.");
            }
            else
            {
                existingDevice.Identifier = deviceAux.Identifier;
                existingDevice.WIFIMAC = deviceAux.WIFIMAC;
                existingDevice.CellphoneNumber = deviceAux.CellphoneNumber;
                existingDevice.OSVersionId = deviceAux.OSVersionId;
                existingDevice.ModelId = deviceAux.ModelId;
                existingDevice.IsAutoDateTime = deviceAux.IsAutoDateTime;
                existingDevice.IsSpoofingGPS = deviceAux.IsSpoofingGPS;
                existingDevice.IsRootedJailbreaked = deviceAux.IsRootedJailbreaked;
                existingDevice.AppVersionId = deviceAux.AppVersionId;
                existingDevice.AssignedAmigoTenantTUserId = deviceAux.AssignedAmigoTenantTUserId;
                existingDevice.Update(message.UserId);
            }


            //if is not valid
            if (deviceAux.HasErrors) return deviceAux.ToResult();


            _deviceRepository.Update(existingDevice);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new DeviceUpdated() { DeviceId = existingDevice.DeviceId});

            //Return result
            return existingDevice.ToResult();
        }

    }







}
