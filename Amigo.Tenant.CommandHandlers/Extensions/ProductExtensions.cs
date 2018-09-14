using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Extensions
{
    public static class ProductExtensions
    {
        public static async Task<Product> GetProduct(this IRepository<Product> repository, int productId)
        {
            var first = await repository.FirstAsync(x => x.ProductId == productId);
            return first;
        }
     
    }
}
