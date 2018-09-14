
using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;
using Amigo.Tenant.Commands.Tracking.Approve;

namespace Amigo.Tenant.Commands.Tracking.Moves
{
   public class UpdateAmigoTenantServiceAckCommand : MobileCommandBase, IAsyncRequest<CommandResult>
    {
        public int AmigoTenantTServiceId { get; set; }
        public List<int?> AmigoTenantTServiceIdList { get; set; }
        public string AcknowledgeBy { get; set; }
        public DateTimeOffset? ServiceAcknowledgeDate { get; set; }
        public string ServiceAcknowledgeDateTZ { get; set; }
        public DateTime? ServiceAcknowledgeDateUTC { get; set; }
        public bool? IsAknowledged { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Username { get; set; }
        public string Request { get; set; }
        public bool? IncludeRequestLog { get; set; }
    }
}
