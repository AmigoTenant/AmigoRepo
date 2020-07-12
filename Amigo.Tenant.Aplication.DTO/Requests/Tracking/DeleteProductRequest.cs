using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class DeleteProductRequest : AuditBaseRequest
    {
        public int ProductId { get; set; }

    }
}
