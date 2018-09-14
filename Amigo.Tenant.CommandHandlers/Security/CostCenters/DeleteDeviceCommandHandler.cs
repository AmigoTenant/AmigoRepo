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
using Amigo.Tenant.Commands.Security.CostCenter;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Security.CostCenters
{

    public class DeleteCostCenterCommandHandler : IAsyncCommandHandler<DeleteCostCenterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CostCenter> _costCenterRepository;

        public DeleteCostCenterCommandHandler(
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

        public async Task<CommandResult> Handle(DeleteCostCenterCommand message)
        {
            //Validate using domain models
            CostCenter costCenterAux = new CostCenter();
            costCenterAux = _mapper.Map<DeleteCostCenterCommand, CostCenter>(message);

            //validate code uniqueness
            var existingCostCenter = await _costCenterRepository.GetCostCenter(message.CostCenterId);

            if (existingCostCenter == null)
            {
                costCenterAux.AddError("Cost Center Id not found.");
            }

            //if is not valid
            if (costCenterAux.HasErrors)
                return costCenterAux.ToResult();

            _costCenterRepository.Delete(existingCostCenter);

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new CostCenterDeleted() { CostCenterId = existingCostCenter.CostCenterId });

            //Return result
            return existingCostCenter.ToResult();
        }

    }

}
