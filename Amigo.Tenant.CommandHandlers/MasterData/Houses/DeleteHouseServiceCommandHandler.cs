using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.Commands.MasterData.Houses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandHandlers.Common;

namespace Amigo.Tenant.CommandHandlers.MasterData.Houses
{
    public class DeleteHouseServiceCommandHandler : IAsyncCommandHandler<DeleteHouseServiceCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HouseService> _houseServiceRepository;
        private readonly IRepository<HouseServicePeriod> _houseServicePeriodRepository;
        private readonly IRepository<ServicePeriod> _servicePeriodRepository;

        public DeleteHouseServiceCommandHandler(IBus bus, IMapper mapper, IUnitOfWork unitOfWork, 
            IRepository<HouseService> houseServiceRepository,
            IRepository<HouseServicePeriod> houseServicePeriodRepository,
            IRepository<ServicePeriod> servicePeriodRepository)
        {
            _bus = bus;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _houseServiceRepository = houseServiceRepository;
            _houseServicePeriodRepository = houseServicePeriodRepository;
            _servicePeriodRepository = servicePeriodRepository;
        }

        public async Task<CommandResult> Handle(DeleteHouseServiceCommand message)
        {
            HouseService entity = _mapper.Map<DeleteHouseServiceCommand, HouseService>(message);

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            if (await _servicePeriodRepository.AnyAsync(p => p.HouseServicePeriod.HouseServiceId == message.HouseServiceId))
            {
                entity.AddError("Cannot delete Service, there are registered service receipts.");
                return entity.ToResult();
            }
            // Delete header HouseService
            entity.RowStatus = false;
            entity.Update(message.UserId);
            _houseServiceRepository.UpdatePartial(entity, 
                new string[] { "RowStatus", "UpdatedBy", "UpdatedDate" });

            // Delete detail HouseServicePeriods
            (await _houseServicePeriodRepository.ListAsync(p => p.HouseServiceId == message.HouseServiceId))
                .ToList()
                .ForEach((HouseServicePeriod p) => { p.RowStatus = false; });

            await _unitOfWork.CommitAsync();

            return entity.ToResult();
        }
    }
}
