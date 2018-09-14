using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Tracking;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class MoveExtensions
    {
        public static async Task<bool> ExitsForDriverAndCostCenter(this IRepository<Move> repository,int driverId, int costcenterId)
        {
            return await repository.FirstAsync(x => x.DriverId == driverId && x.CostId == costcenterId) != null;
        }
    }
}
