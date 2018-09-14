using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class LocationCoordinateExtensions
    {

        public static async Task<IEnumerable<LocationCoordinate>> GetLocationCoordinates(this IRepository<LocationCoordinate> repository, int locationId)
        {
            var coordinates = await repository.ListAsync(x => x.LocationId == locationId);
            return coordinates;
        }

    }
}
