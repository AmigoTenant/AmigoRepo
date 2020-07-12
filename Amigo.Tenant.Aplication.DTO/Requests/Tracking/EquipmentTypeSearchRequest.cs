using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public   class EquipmentTypeSearchRequest : PagedRequest
    {
        public string Name { get; set; }

    }
}
