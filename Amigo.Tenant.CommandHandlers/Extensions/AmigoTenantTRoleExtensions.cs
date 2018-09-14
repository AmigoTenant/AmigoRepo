using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class AmigoTenantTRoleExtensions
    {
        public static async Task<bool> ExistsByCodeName(this IRepository<AmigoTenantTRole> repository,string code, string name)
        {
            return await repository.FirstAsync(x => x.Code == code && x.RowStatus.Value) != null;
        }
    }
}
