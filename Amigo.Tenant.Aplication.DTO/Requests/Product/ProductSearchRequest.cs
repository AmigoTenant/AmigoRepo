using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Product
{
    class ProductSearchRequest :PagedRequest
    {
        public string Name { get; set; }


    }
}
