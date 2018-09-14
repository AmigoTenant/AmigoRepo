using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.Security;
using System.Linq;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Security.CostCenter;

namespace Amigo.Tenant.CommandHandlers.Security.CostCenters
{

    public class RegisterCostCenterCommandHandler : IAsyncCommandHandler<RegisterCostCenterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CostCenter> _costCenterRepository;

        public RegisterCostCenterCommandHandler(
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

        public async Task<CommandResult> Handle(RegisterCostCenterCommand message)
        {
            //Validate using domain models
            var costCenter = _mapper.Map<RegisterCostCenterCommand, CostCenter>(message);

            costCenter.RowStatus = true;
            costCenter.Creation(message.UserId);

            //if is not valid
            if (costCenter.HasErrors)
                return costCenter.ToResult();

            //Insert
            _costCenterRepository.Add(costCenter);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new CostCenterRegistered() { CostCenterId = costCenter.CostCenterId });

            //Return result
            return costCenter.ToResult();
        }


    }
}
