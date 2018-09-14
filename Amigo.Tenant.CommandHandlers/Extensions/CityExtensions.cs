using System;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{

    public static class CityExtensions
    {
        public static async Task<int> GetCityId(this IRepository<City> repository, string code)
        {
            var first = await repository.FirstAsync(x => x.Code == code);
            return first.CityId;
        }
    }

}
