using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using System.Linq;

namespace Amigo.Tenant.CommandHandlers.Tracking.Locations
{

    public class DeleteLocationCoordinatesCommandHandler : IAsyncCommandHandler<DeleteLocationCoordinatesCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<LocationCoordinate> _locationCoordinateRepository;


        public DeleteLocationCoordinatesCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Location> locationRepository,
            IRepository<LocationCoordinate> locationCoordinateRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _locationRepository = locationRepository;
            _locationCoordinateRepository = locationCoordinateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteLocationCoordinatesCommand message)
        {
            Location location = null;

            //Get ParentLocationId
            location = await _locationRepository.GetLocation(message.Code);

            var coordinates = await _locationCoordinateRepository.GetLocationCoordinates(location.LocationId);
            
            coordinates.ToList().ForEach(p => _locationCoordinateRepository.Delete(p));

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new LocationCoordinateDeleted() { LocationId = location .LocationId});

            //Return result
            return location.ToResult();
        }

    }


}
