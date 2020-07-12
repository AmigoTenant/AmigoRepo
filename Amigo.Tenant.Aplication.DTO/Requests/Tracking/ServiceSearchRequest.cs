using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class ServiceSearchRequest : PagedRequest
    {
        public string ServiceTypeCode { get; set; }

    }
}
