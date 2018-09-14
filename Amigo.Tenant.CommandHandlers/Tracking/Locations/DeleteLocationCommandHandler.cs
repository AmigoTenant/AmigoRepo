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
    public class DeleteLocationCommandHandler : IAsyncCommandHandler<DeleteLocationCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Location> _locationRepository;

        public DeleteLocationCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Location> locationRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteLocationCommand message)
        {
            //Validate using domain models
            Location locationAux = new Location();
            locationAux = _mapper.Map<DeleteLocationCommand, Location>(message);

            //validate code uniqueness
            var existingLocation = await _locationRepository.GetLocation(message.Code);

            if (existingLocation == null)
            {
                locationAux.AddError("Location code not found.");
            }
            else
            {
                existingLocation.RowStatus = false;
            }


            //if is not valid
            if (locationAux.HasErrors) return locationAux.ToResult();
            
            _locationRepository.UpdatePartial(existingLocation, new string[] { "RowStatus" });

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new LocationDeleted() { LocationId = existingLocation.LocationId });

            //Return result
            return existingLocation.ToResult();
        }

    }
}
