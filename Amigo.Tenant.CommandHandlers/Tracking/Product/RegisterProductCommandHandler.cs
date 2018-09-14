using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Commands.Tracking.Product;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;

namespace Amigo.Tenant.CommandHandlers.Tracking.Products
{
    public class RegisterProductCommandHandler : IAsyncCommandHandler<RegisterProductCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepository;

        public RegisterProductCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Product> productRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterProductCommand message)
        {
            //Validate using domain models
            var product = _mapper.Map<RegisterProductCommand, Product>(message);

            product.RowStatus = true;
            product.Creation(message.UserId);

            //if is not valid
            if (product.HasErrors) return product.ToResult();

            //Insert
            _productRepository.Add(product);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new ProductRegistered() {  ProductId = product.ProductId});

            //Return result
            return product.ToResult();
        }
    }
}
