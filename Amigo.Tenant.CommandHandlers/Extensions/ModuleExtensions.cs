using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{

    public static class ModuleExtensions
    {

        public static async Task<Module> GetModule(this IRepository<Module> repository, string code)
        {
            var first = await repository.FirstOrDefaultAsync(x => x.Code == code);
            return first;
        }

    }

}
