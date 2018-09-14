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
    public class RegisterLocationCommandHandler : IAsyncCommandHandler<RegisterLocationCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<LocationType> _locationTypeRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<LocationCoordinate> _locationCoordinateRepository;

        public RegisterLocationCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Location> locationRepository,
            IRepository<LocationCoordinate> locationCoordinateRepository,
            IRepository<LocationType> locationTypeRepository,
            IRepository<City> cityRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _locationRepository = locationRepository;
            _locationCoordinateRepository = locationCoordinateRepository;
            _locationTypeRepository = locationTypeRepository;
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterLocationCommand message)
        {
            //Validate using domain models
            var location = _mapper.Map<RegisterLocationCommand,Location>(message);

            //validate code uniqueness
            var existinglocation = await _locationRepository.GetLocation(message.Code);
            if (existinglocation != null) location.AddError("A location already exists for this code.");


            if (!string.IsNullOrEmpty(message.LocationTypeCode))
            {
                //Get LocationTypeId
                int locationTypeId = await _locationTypeRepository.GetLocationTypeId(message.LocationTypeCode);
                location.LocationTypeId = locationTypeId;
            }

            if (!string.IsNullOrEmpty(message.ParentLocationCode))
            {
                //Get ParentLocationId
                int parentLocationId = await _locationRepository.GetLocationId(message.ParentLocationCode);
                location.ParentLocationId = parentLocationId;
            }


            if (!string.IsNullOrEmpty(message.CityCode))
            {
                //Get CityId
                int cityId = await _cityRepository.GetCityId(message.CityCode);
                location.CityId = cityId;
            }

            location.RowStatus = true;
            location.Creation(message.UserId);

            //if is not valid
            if (location.HasErrors) return location.ToResult();

            #region Coordinates

            //-------------------------------------------
            //          Insert  coordinates
            //-------------------------------------------

            if (message.Coordinates != null && message.Coordinates.Count > 0)
            {
                foreach (var coordinate in message.Coordinates)
                {
                    var locationCoordinate = _mapper.Map<RegisterLocationCoordinateItem, LocationCoordinate>(coordinate);
                    location.LocationCoordinates.Add(locationCoordinate);
                }

                location.HasGeofence = true;
            }
            else
            {
                location.HasGeofence = false;
            }

            #endregion


            //Insert
            _locationRepository.Add(location);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new LocationRegistered() { LocationId = location.LocationId });

            //Return result
            return location.ToResult();            
        }


    }
}
