using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class PermissionExtensions
    {
        public static async Task<bool> Exists(this IRepository<Permission> repository,int? amigoTenantTRoleId, int? actionId)
        {
            return await repository.FirstAsync(x => x.AmigoTenantTRoleId == amigoTenantTRoleId && x.ActionId == actionId) != null;
        }
    }
}
