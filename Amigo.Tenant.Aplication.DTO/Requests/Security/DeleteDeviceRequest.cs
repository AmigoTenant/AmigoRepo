using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class DeleteDeviceRequest : AuditBaseRequest
    {
        public int DeviceId { get; set; }

    }
}
