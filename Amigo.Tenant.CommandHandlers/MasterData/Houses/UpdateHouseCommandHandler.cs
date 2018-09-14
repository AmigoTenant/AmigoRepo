using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.MasterData;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.MasterData.Houses
{
    public class UpdateHouseCommandHandler : IAsyncCommandHandler<UpdateHouseCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<House> _mainTenantRepository;

        public UpdateHouseCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<House> deviceRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _mainTenantRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateHouseCommand message)
        {
            //Validate using domain models
            House entity = _mapper.Map<UpdateHouseCommand, House>(message);

            //if is not valid
            //if (entity.HasErrors) return entity.ToResult();
            entity.UpdatedBy = message.UserId;
            entity.UpdatedDate = DateTime.Now;

            _mainTenantRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new HouseUpdated() { Id = entity.HouseId });

            //Return result
            return entity.ToResult();
        }
    }
}
