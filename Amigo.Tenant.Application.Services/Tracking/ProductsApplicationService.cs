using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Product;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Tracking.Product;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class ProductsApplicationService: IProductsApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ProductDTO> _productDataAcces;

        public ProductsApplicationService(IBus bus,
            IQueryDataAccess<ProductDTO> productDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _productDataAcces = productDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> RegisterProductAsync(RegisterProductRequest newProduct)
        {

            //Map to Command
            var command = _mapper.Map<RegisterProductRequest, RegisterProductCommand>(newProduct);

            var response = await ValidateEntityRegister(newProduct);

            if (response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO> UpdateProductAsync(UpdateProductRequest product)
        {
            //Map to Command
            var command = _mapper.Map<UpdateProductRequest, UpdateProductCommand>(product);

            var response = await ValidateEntityUpdate(product);

            if (response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO> DeleteProductAsync(DeleteProductRequest product)
        {
            //Map to Command
            var command = _mapper.Map<DeleteProductRequest, DeleteProductCommand>(product);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<ProductDTO>>> SearchProductsByNameAsync(ProductSearchRequest search)
        {
            List<OrderExpression<ProductDTO>> orderExpressionList = new List<OrderExpression<ProductDTO>>();
            orderExpressionList.Add(new OrderExpression<ProductDTO>(OrderType.Asc, p => p.Name));

            Expression<Func<ProductDTO, bool>> queryFilter = c => c.RowStatus==true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            if (!string.IsNullOrEmpty(search.ShortName))
                queryFilter = queryFilter.And(p => p.ShortName.Contains(search.ShortName));

            if (!string.IsNullOrEmpty(search.IsHazardous))
                queryFilter = queryFilter.And(p => p.IsHazardous.Contains(search.IsHazardous));

            var product = await _productDataAcces.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<ProductDTO>()
            {
                Items = product.Items,
                PageSize = product.PageSize,
                Page = product.Page,
                Total = product.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<ProductTypeAheadDTO>>> SearchProductAllTypeAhead(string name)
        {
            Expression<Func<ProductDTO, bool>> queryFilter = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(name));
            }
            var list = (await _productDataAcces.ListAsync(queryFilter)).ToList();
            var typeAheadList = list.Select(x => new ProductTypeAheadDTO() { ProductId = x.ProductId, Code = x.Code, Name = x.Name }).ToList();
            return ResponseBuilder.Correct(typeAheadList);

        }

        public async Task<ResponseDTO> ValidateEntityRegister(RegisterProductRequest request)
        {
            Expression<Func<ProductDTO, bool>> queryFilter = p => true;
            var result = "";

            queryFilter = queryFilter.And(p => p.Code == request.Code);
            queryFilter = queryFilter.Or(p => p.RowStatus == true && (p.Name == request.Name || p.ShortName == request.ShortName));

            var product = await _productDataAcces.FirstOrDefaultAsync(queryFilter);

            if (product != null)
                result = "Product already Exists";

            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(result),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(result) ? "Ok" : "Error",
                Message = result
            });

            return response;
        }

        public async Task<ResponseDTO> ValidateEntityUpdate(UpdateProductRequest request)
        {
            Expression<Func<ProductDTO, bool>> queryFilter = p => p.RowStatus;
            var result = "";

            queryFilter = p => p.RowStatus;
            queryFilter = queryFilter.And(p => p.ProductId != request.ProductId);
            queryFilter = queryFilter.And(p => p.Name == request.Name || p.ShortName == request.ShortName);
            
            var product = await _productDataAcces.FirstOrDefaultAsync(queryFilter);

            if (product != null)
                result = "There is a product that contain some information that your are trying to update, try with another.";

            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(result),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(result) ? "Ok" : "Error",
                Message = result
            });

            return response;
        }
    }
}
