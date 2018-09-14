using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Product;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IProductsApplicationService
    {
        Task<ResponseDTO> RegisterProductAsync(RegisterProductRequest newproduct);
        Task<ResponseDTO<PagedList<ProductDTO>>> SearchProductsByNameAsync(ProductSearchRequest search);
        Task<ResponseDTO<List<ProductTypeAheadDTO>>> SearchProductAllTypeAhead(string name);
        Task<ResponseDTO> UpdateProductAsync(UpdateProductRequest product);
        Task<ResponseDTO> DeleteProductAsync(DeleteProductRequest product);
    }
}
