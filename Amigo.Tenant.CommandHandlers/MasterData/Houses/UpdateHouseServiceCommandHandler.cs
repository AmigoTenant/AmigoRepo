using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.MasterData.Houses
{
    public class UpdateHouseServiceCommandHandler : IAsyncCommandHandler<UpdateHouseServiceCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HouseService> _houseServiceRepository;

        public UpdateHouseServiceCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<HouseService> houseServiceRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _houseServiceRepository = houseServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateHouseServiceCommand message)
        {
            HouseService entity = _mapper.Map<UpdateHouseServiceCommand, HouseService>(message);

            entity.Update(message.UserId);

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            _houseServiceRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            //await _bus.PublishAsync(new HouseUpdated() { Id = entity.HouseId });

            //Return result
            return entity.ToResult();
        }
    }
}
