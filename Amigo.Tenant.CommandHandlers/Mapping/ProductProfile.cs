using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Tracking.Product;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.CommandHandlers.Mapping
{

    public class ProductProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterProductCommand, Product>();
            Mapper.Register<UpdateProductCommand, Product>();
            Mapper.Register<DeleteProductCommand, Product>();
        }
    }


}
