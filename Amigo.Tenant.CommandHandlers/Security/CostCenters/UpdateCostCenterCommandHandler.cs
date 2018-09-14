using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.Commands.Security.CostCenter;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Security.CostCenters
{

    public class UpdateCostCenterCommandHandler : IAsyncCommandHandler<UpdateCostCenterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CostCenter> _costCenterRepository;

        public UpdateCostCenterCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<CostCenter> costCenterRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _costCenterRepository = costCenterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateCostCenterCommand message)
        {
            //Validate using domain models
            CostCenter costCenterAux = new CostCenter();
            costCenterAux = _mapper.Map<UpdateCostCenterCommand, CostCenter>(message);

            //validate code uniqueness
            var existingCostCenter = await _costCenterRepository.GetCostCenter(message.CostCenterId);

            if (existingCostCenter == null)
            {
                costCenterAux.AddError("Device Id not found.");
            }
            else
            {
                existingCostCenter.Code = costCenterAux.Code;
                existingCostCenter.Name = costCenterAux.Name;
                existingCostCenter.Update(message.UserId);
            }


            //if is not valid
            if (costCenterAux.HasErrors)
                return costCenterAux.ToResult();


            _costCenterRepository.Update(existingCostCenter);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new DeviceUpdated() { DeviceId = existingCostCenter.CostCenterId });

            //Return result
            return existingCostCenter.ToResult();
        }

    }







}
