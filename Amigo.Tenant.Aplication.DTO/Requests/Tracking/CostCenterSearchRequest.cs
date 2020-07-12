
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class CostCenterSearchRequest : PagedRequest
    {
        public string Code
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }
}
