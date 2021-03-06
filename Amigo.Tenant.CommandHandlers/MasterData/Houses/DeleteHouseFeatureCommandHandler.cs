﻿using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
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
    public class DeleteHouseFeatureCommandHandler : IAsyncCommandHandler<DeleteHouseFeatureCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<HouseFeature> _houseFeatRepository;
        private readonly IRepository<House> _houseRepository;
        private readonly IRepository<EntityStatus> _statusRepository;

        public DeleteHouseFeatureCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<HouseFeature> houseFeatRepository,
            IRepository<House> houseRepository,
            IRepository<EntityStatus> statusRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _houseFeatRepository = houseFeatRepository;
            _houseRepository = houseRepository;
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteHouseFeatureCommand message)
        {
            HouseFeature entity = _mapper.Map<DeleteHouseFeatureCommand, HouseFeature>(message);

            entity.RowStatus = false;
            entity.Update(message.UserId);

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            var repairStatus = await _statusRepository.FirstOrDefaultAsync(p => p.EntityCode == "HO" && p.Code == "REPAIR");
            var repairStatusId = repairStatus == null ? 0 : repairStatus.EntityStatusId;

            var house = await _houseRepository
                .FirstOrDefaultAsync(p => p.HouseId == message.HouseId);

            var features = await _houseFeatRepository
                .ListAsync(w => w.RowStatus
                        && w.HouseId == message.HouseId
                        && w.HouseFeatureStatusId != repairStatusId
                        && w.HouseFeatureId != message.HouseFeatureId
                        && !w.Feature.IsAllHouse);

            var featureAllHouse = await _houseFeatRepository
                .FirstOrDefaultAsync(w => w.RowStatus
                        && w.HouseId == message.HouseId
                        && w.Feature.IsAllHouse);

            var sumRentPrice = features.Sum(p => p.RentPrice);

            featureAllHouse.RentPrice = sumRentPrice;
            house.RentPrice = sumRentPrice;
            house.Update(message.UserId);

            _houseFeatRepository.UpdatePartial(featureAllHouse, new string[] { "HouseFeatureId",
                    "RentPrice",
                    "UpdatedDate",
                    "UpdatedBy"
                    });

            _houseRepository.UpdatePartial(house, new string[] { "HouseId",
                    "RentPrice",
                    "UpdatedDate",
                    "UpdatedBy"
                    });

            _houseFeatRepository.UpdatePartial(entity, new string[] { "HouseFeatureId",
                    "RowStatus",
                    "UpdatedDate",
                    "UpdatedBy"
                    });
            //_houseRepository.Delete(house);

            await _unitOfWork.CommitAsync();
            //await _bus.PublishAsync(new HouseDeleted() { Id = house.HouseId });

            return entity.ToResult();
        }
    }
}
