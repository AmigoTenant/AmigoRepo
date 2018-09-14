using System.Threading.Tasks;
using System.Web.Http;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Application.Services.Interfaces.Tracking;

namespace XPO.ShuttleTracking.Application.Services.WebApi.Controllers
{    
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsApplicationService _productsApplicationService;

        public ProductsController(IProductsApplicationService productsApplicationService)
        {
            _productsApplicationService = productsApplicationService;
        }

        [HttpPost,Route("search")]
        public async Task<ResponseDTO<PagedList<ProductResponse>>> Search(ProductSearchRequest search)
        {
            var resp = await _productsApplicationService.SearchProductsByNameAsync(search);
            return resp;            
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody]RegisterProductRequest product)
        {
            var resp = await _productsApplicationService.RegisterProductAsync(product);
            return resp;
        }
    }
}
