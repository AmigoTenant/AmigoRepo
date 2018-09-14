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
    public class UpdateLocationCommandHandler : IAsyncCommandHandler<UpdateLocationCommand>
    {


        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<LocationType> _locationTypeRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<LocationCoordinate> _locationCoordinateRepository;

        public UpdateLocationCommandHandler(
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

        public async Task<CommandResult> Handle(UpdateLocationCommand message)
        {
            //Validate using domain models
            Location locationAux = new Location();
            locationAux = _mapper.Map<UpdateLocationCommand, Location>(message);

            //validate code uniqueness
            var existingLocation = await _locationRepository.GetLocation(message.Code);

            if (existingLocation == null)
            {
                locationAux.AddError("Location code not found.");
            }
            else
            {

                existingLocation.Name = locationAux.Name;
                existingLocation.Latitude = locationAux.Latitude;
                existingLocation.Longitude = locationAux.Longitude;
                existingLocation.Coordinate = locationAux.Coordinate;
                existingLocation.Address1 = locationAux.Address1;
                existingLocation.Address2 = locationAux.Address2;
                existingLocation.ZipCode = locationAux.ZipCode;

                if (!string.IsNullOrEmpty(message.LocationTypeCode))
                {
                    //Get LocationTypeId
                    int locationTypeId = await _locationTypeRepository.GetLocationTypeId(message.LocationTypeCode);
                    existingLocation.LocationTypeId = locationTypeId;
                }

                if (!string.IsNullOrEmpty(message.ParentLocationCode))
                {
                    //Get ParentLocationId
                    int parentLocationId = await _locationRepository.GetLocationId(message.ParentLocationCode);
                    existingLocation.ParentLocationId = parentLocationId;
                }


                if (!string.IsNullOrEmpty(message.CityCode))
                {
                    //Get CityId
                    int cityId = await _cityRepository.GetCityId(message.CityCode);
                    existingLocation.CityId = cityId;
                }
            }
                
            
            //if is not valid
            if (locationAux.HasErrors) return locationAux.ToResult();


            #region Coordinates
            
            //-------------------------------------------
            //          Delete coordinates
            //-------------------------------------------

            var coordinates = await _locationCoordinateRepository.GetLocationCoordinates(existingLocation.LocationId);
            coordinates.ToList().ForEach(p => _locationCoordinateRepository.Delete(p));

            //-------------------------------------------
            //          Insert  coordinates
            //-------------------------------------------
            if (message.Coordinates != null && message.Coordinates.Count > 0)
            {
                foreach (var coordinate in message.Coordinates)
                {
                    var locationCoordinate = _mapper.Map<RegisterLocationCoordinateItem, LocationCoordinate>(coordinate);
                    existingLocation.LocationCoordinates.Add(locationCoordinate);
                }

                existingLocation.HasGeofence = true;
            }
            else
            {
                existingLocation.HasGeofence = false;
            }

            #endregion


            _locationRepository.Update(existingLocation);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new LocationUpdated() { LocationId = existingLocation.LocationId });

            //Return result
            return existingLocation.ToResult();
        }

    }
}
