using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class DeleteModuleRequest: AuditBaseRequest
    {
        public string Code { get; set; }

    }
}
