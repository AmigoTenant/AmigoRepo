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
    public class RegisterHouseCommandHandler : IAsyncCommandHandler<RegisterHouseCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<House> _houseRepository;
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<HouseFeature> _houseFeatRepository;
        private readonly IRepository<EntityStatus> _statusRepository;

        public RegisterHouseCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<House> HouseRepository,
            IRepository<Feature> featureRepository,
            IRepository<HouseFeature> houseFeatRepository,
            IRepository<EntityStatus> statusRepository,
            IUnitOfWork unitOfWork)

        {
            _bus = bus;
            _mapper = mapper;
            _houseRepository = HouseRepository;
            _featureRepository = featureRepository;
            _statusRepository = statusRepository;
            _houseFeatRepository = houseFeatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterHouseCommand message)
        {
            //Validate using domain models
            var entity = _mapper.Map<RegisterHouseCommand, House>(message);

            entity.RowStatus = true;
            entity.Creation(message.UserId);

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();
            var forRentStatusId = (await _statusRepository.FirstOrDefaultAsync(p => p.EntityCode == "HO" && p.Code == "FOR RENT"))?.EntityStatusId;
            var completeFeatureId = (await _featureRepository.FirstOrDefaultAsync(w => w.IsAllHouse && w.HouseTypeId == message.HouseTypeId))?.FeatureId;

            var initialFeature = HouseFeature.Create(completeFeatureId.GetValueOrDefault(), message.UserId, forRentStatusId.GetValueOrDefault());

            //Insert
            _houseFeatRepository.Add(initialFeature);
            _houseRepository.Add(entity);

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            //await _bus.PublishAsync(new HouseRegistered() { Id = entity.HouseId });

            //Return result
            if (entity.HouseId != 0)
            {
                message.HouseId = entity.HouseId;
            }

            return entity.ToRegisterdResult().WithId(entity.HouseId);
            //return entity.ToResult();
        }
    }
}
