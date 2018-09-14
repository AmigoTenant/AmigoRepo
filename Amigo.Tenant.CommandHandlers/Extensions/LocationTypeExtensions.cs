using System;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{

    public static class LocationTypeExtensions
    {
        public static async Task<int> GetLocationTypeId(this IRepository<LocationType> repository, string code)
        {
            var first = await repository.FirstAsync(x => x.Code == code);
            return first.LocationTypeId;
        }
    }
}
