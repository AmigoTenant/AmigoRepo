using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{

    public static class ActionExtensions
    {

        public static async Task<Action> GetAction(this IRepository<Action> repository, string code)
        {
            var first = await repository.FirstAsync(x => x.Code == code);
            return first;
        }

    }
}
