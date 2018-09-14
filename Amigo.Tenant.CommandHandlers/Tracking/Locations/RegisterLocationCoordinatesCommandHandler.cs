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

namespace Amigo.Tenant.CommandHandlers.Tracking.Locations
{
    public class RegisterLocationCoordinatesCommandHandler : IAsyncCommandHandler<RegisterLocationCoordinatesCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<LocationCoordinate> _locationCoordinateRepository;


        public RegisterLocationCoordinatesCommandHandler(
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

        public async Task<CommandResult> Handle(RegisterLocationCoordinatesCommand message)
        {
            LocationCoordinate locationCoordinate = null;

            foreach (var registerLocationCoordinate in message.RegisterLocationCoordinatesList)
            {
                //Validate using domain models
                locationCoordinate = _mapper.Map<RegisterLocationCoordinateItem, LocationCoordinate>(registerLocationCoordinate);


                if (!string.IsNullOrEmpty(registerLocationCoordinate.LocationCode))
                {
                    //Get LocationTypeId
                    int locationId = await _locationRepository.GetLocationId(registerLocationCoordinate.LocationCode);
                    locationCoordinate.LocationId = locationId;
                }

                //if is not valid
                if (locationCoordinate.HasErrors) return locationCoordinate.ToResult();


                //Insert
                _locationCoordinateRepository.Add(locationCoordinate);

                await _unitOfWork.CommitAsync();

                //Publish bussines Event
                await _bus.PublishAsync(new LocationCoordinateRegistered() { LocationCoordinateId = locationCoordinate.LocationCoordinateId });

            }

            //Return result
            return locationCoordinate.ToResult();
        }

    }
}
