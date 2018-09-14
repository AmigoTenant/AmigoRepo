using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.Device
{
    public class UpdateDeviceCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int DeviceId { get; set; }
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
        public bool? RowStatus { get; set; }
    }
}
