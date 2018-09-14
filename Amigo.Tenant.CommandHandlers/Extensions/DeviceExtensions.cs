using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class DeviceExtensions
    {
        public static async Task<Device> GetDevice(this IRepository<Device> repository, int deviceId)
        {
            var first = await repository.FirstAsync(x => x.DeviceId == deviceId);
            return first;
        }
     
    }
}
