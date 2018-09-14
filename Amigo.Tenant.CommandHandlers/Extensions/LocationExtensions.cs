using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class LocationExtensions
    {
        //public static async Task<Location> ExistingLocationByCode(this IRepository<Location> repository, string code)
        //{
        //    var location = await repository.FirstAsync(x => x.Code == code);
        //    return location;
        //}


        public static async Task<Location> GetLocation(this IRepository<Location> repository, string code)
        {
            var first = await repository.FirstAsync(x => x.Code == code);
            return first;
            //return first.LocationId;
        }


        public static async Task<int> GetLocationId(this IRepository<Location> repository, string code)
        {
            var first = await repository.FirstAsync(x => x.Code == code);
            return first.LocationId;
        }
    }
}
