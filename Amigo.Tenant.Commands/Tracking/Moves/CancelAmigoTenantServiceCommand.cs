
using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;
using Amigo.Tenant.Commands.Tracking.Approve;

namespace Amigo.Tenant.Commands.Tracking.Moves
{
   public class CancelAmigoTenantServiceCommand :MobileCommandBase, IAsyncRequest<CommandResult>
    {
        public int AmigoTenantTServiceId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Username { get; set; } // value comes from token
        public bool? RowStatus { get; set; }
        public int UserId { get; set; } //
        public bool? IncludeRequestLog { get; set; }
    }
}
