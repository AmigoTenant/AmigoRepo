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
    public class DeleteHouseCommandHandler : IAsyncCommandHandler<DeleteHouseCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<House> _houseRepository;

        public DeleteHouseCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<House> houseRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _houseRepository = houseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteHouseCommand message)
        {
            var entity = _mapper.Map<DeleteHouseCommand, House>(message);
            if (entity.HasErrors) return entity.ToResult();

            //_houseRepository.Delete(house);
            entity.RowStatus = false;
            entity.Update(message.UserId);
            _houseRepository.UpdatePartial(entity, new string[] { "HouseId",
                    "RowStatus",
                    "UpdatedDate",
                    "UpdatedBy",
                    });

            await _unitOfWork.CommitAsync();

            await _bus.PublishAsync(new HouseDeleted() { Id = entity.HouseId });

            return entity.ToResult();
        }
    }
}
