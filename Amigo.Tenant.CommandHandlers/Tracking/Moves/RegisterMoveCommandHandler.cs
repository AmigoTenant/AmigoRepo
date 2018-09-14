using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Tracking.Moves
{
    public class RegisterMoveCommandHandler: IAsyncCommandHandler<RegisterMoveCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Move> _movesRepository;

        public RegisterMoveCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<Move> movesRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _movesRepository = movesRepository;
            _unitOfWork = unitOfWork;
        }                

        public async Task<CommandResult> Handle(RegisterMoveCommand message)
        {
            //Validate using domain models
            var move = _mapper.Map<RegisterMoveCommand,Move>(message);

            var alreadyExists = await _movesRepository.ExitsForDriverAndCostCenter(message.DriverId,message.CostCenterId);
            if (alreadyExists) move.AddError("A move already exists for this driver.");
            
            //more bussines rules
            //more bussines rules

            //if is not valid
            if (move.HasErrors) return move.ToResult();

            //Insert
            _movesRepository.Add(move);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new MoveRegistered() { MoveId = move.Id });

            //Return result
            return move.ToResult();
        }
    }
}
