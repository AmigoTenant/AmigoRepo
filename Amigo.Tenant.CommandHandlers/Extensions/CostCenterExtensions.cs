using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class CostCenterExtensions
    {
        public static async Task<CostCenter> GetCostCenter(this IRepository<CostCenter> repository, int costCenterId)
        {
            var first = await repository.FirstAsync(x => x.CostCenterId == costCenterId);
            return first;
        }

    }
}
