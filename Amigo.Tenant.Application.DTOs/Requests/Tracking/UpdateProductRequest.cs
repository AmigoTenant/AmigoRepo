using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class UpdateProductRequest : AuditBaseRequest
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsHazardous { get; set; }
        public bool? IsHazardousBool { get; set; }

    }
}
