using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class RegisterDeviceRequest : AuditBaseRequest
    {

        public string Identifier { get; set; }
        public string WIFIMAC { get; set; }
        public string CellphoneNumber { get; set; }
        public int? OSVersionId { get; set; }
        public int? ModelId { get; set; }
        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }
        public int? AppVersionId { get; set; }
        public int? AssignedAmigoTenantTUserId { get; set; }

    }

}
