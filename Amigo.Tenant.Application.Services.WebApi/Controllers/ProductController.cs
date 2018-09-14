using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Caching.Web.Filters;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;
using Amigo.Tenant.Common;
using static Amigo.Tenant.Common.ConstantsSecurity;
using Amigo.Tenant.Application.Services.WebApi.Filters;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{    
    [RoutePrefix("api/products"),CachingMasterData]
    public class ProductController : ApiController
    {
        private readonly IProductsApplicationService _productsApplicationService;

        public ProductController(IProductsApplicationService productsApplicationService)
        {
            _productsApplicationService = productsApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<ProductDTO>>> Search([FromUri]ProductSearchRequest search)
        {
            var resp = await _productsApplicationService.SearchProductsByNameAsync(search);
            return resp;            
        }

        [HttpGet, Route("searchProductAllTypeAheadByName")]
        public Task<ResponseDTO<List<ProductTypeAheadDTO>>> GetProductAllTypeAhead(string name)
        {
            var resp = _productsApplicationService.SearchProductAllTypeAhead(name);
            return resp;
        }

        [HttpPost, Route("register") , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ProductCreate)]
        public async Task<ResponseDTO> Register(RegisterProductRequest product)
        {
            if (ModelState.IsValid)
            {
                product.IsHazardous = product.IsHazardousBool.Value ? "1":"0" ;
                return await _productsApplicationService.RegisterProductAsync(product);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update") , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ProductUpdate)]
        public async Task<ResponseDTO> Update(UpdateProductRequest product)
        {
            if (ModelState.IsValid)
            {
                product.IsHazardous = product.IsHazardousBool.Value ? "1" : "0";
                return await _productsApplicationService.UpdateProductAsync(product);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete") , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ProductDelete)]
        public async Task<ResponseDTO> Delete(DeleteProductRequest product)
        {
            if (ModelState.IsValid)
            {
                return await _productsApplicationService.DeleteProductAsync(product);
            }
            return ModelState.ToResponse();
        }


    }
}
