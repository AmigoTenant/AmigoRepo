
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class EquipmentSizeSearchRequest : PagedRequest
    {
        public string Name { get; set; }        
    }
}
