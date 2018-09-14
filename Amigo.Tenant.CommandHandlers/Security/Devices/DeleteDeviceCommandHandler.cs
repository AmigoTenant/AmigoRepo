using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using commandModels = Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;

namespace Amigo.Tenant.CommandHandlers.Security.Devices
{

    public class DeleteDeviceCommandHandler : IAsyncCommandHandler<DeleteDeviceCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<commandModels.Device> _deviceRepository;

        public DeleteDeviceCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<commandModels.Device> deviceRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteDeviceCommand message)
        {
            //Validate using domain models
            commandModels.Device deviceAux = new commandModels.Device();
            deviceAux = _mapper.Map<DeleteDeviceCommand, commandModels.Device>(message);

            //validate code uniqueness
            var existingDevice = await _deviceRepository.GetDevice(message.DeviceId);

            if (existingDevice == null)
            {
                deviceAux.AddError("Device Id not found.");
            }
            //else
            //{
            //    existingDevice.RowStatus = false;
            //    existingDevice.Update(message.UserId);
            //}


            //if is not valid
            if (deviceAux.HasErrors) return deviceAux.ToResult();

            //_deviceRepository.UpdatePartial(existingDevice, new string[] { "RowStatus", "UpdatedBy", "UpdatedDate" });
            _deviceRepository.Delete(existingDevice);

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new DeviceDeleted() { DeviceId = existingDevice.DeviceId });

            //Return result
            return existingDevice.ToResult();
        }

    }

}
