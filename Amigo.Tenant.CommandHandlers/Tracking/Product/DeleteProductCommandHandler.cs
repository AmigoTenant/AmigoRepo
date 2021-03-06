﻿using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Commands.Tracking.Product;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandHandlers.Extensions;

namespace Amigo.Tenant.CommandHandlers.Tracking.Products
{

    public class DeleteProductCommandHandler : IAsyncCommandHandler<DeleteProductCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepository;

        public DeleteProductCommandHandler(
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

        public async Task<CommandResult> Handle(DeleteProductCommand message)
        {
            //Validate using domain models
            Product productAux = new Product();
            productAux = _mapper.Map<DeleteProductCommand, Product>(message);

            //validate code uniqueness
            var existingProduct =  await _productRepository.GetProduct(message.ProductId);

            if (existingProduct == null)
            {
                productAux.AddError("Product Id not found.");
            }
            else
            {
                existingProduct.RowStatus = false;
                existingProduct.Update(message.UserId);
            }


            //if is not valid
            if (productAux.HasErrors) return productAux.ToResult();

            _productRepository.UpdatePartial(existingProduct, new string[] { "RowStatus", "UpdatedBy", "UpdatedDate" });

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new ProductDeleted() { ProductId = existingProduct.ProductId });

            //Return result
            return existingProduct.ToResult();

            
        }

    }

}
