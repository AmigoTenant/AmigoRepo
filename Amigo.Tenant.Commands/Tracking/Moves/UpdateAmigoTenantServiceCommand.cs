
using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;
using Amigo.Tenant.Commands.Tracking.Approve;

namespace Amigo.Tenant.Commands.Tracking.Moves
{
   public class UpdateAmigoTenantServiceCommand :MobileCommandBase, IAsyncRequest<CommandResult>
    {
        public int AmigoTenantTServiceId { get; set; }
        public DateTimeOffset? ServiceFinishDate { get; set; }
        public string ServiceFinishDateTZ { get; set; }
        public DateTime? ServiceFinishDateUTC { get; set; }
        public int? DestinationLocationId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ApprovalModified { get; set; }
        public bool? ServiceStatus { get; set; }
        public string[] FieldsToUpdate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public List<RegisterAmigoTenantTServiceChargeCommand> AmigoTenantTServiceCharges;

        public string Username { get; set; } // value comes from token
        public bool? RowStatus { get; set; }
        public int UserId { get; set; } //
        public Guid ServiceOrderNo { get; set; }
        public string Request { get; set; }
        public bool? IncludeRequestLog { get; set; }
        public string ChargeType { get; set; }
        public string ApprovalComments { get; set; }
        public string ChargeNo { get; set; }
        public string DriverComments { get; set; }
        public int? EquipmentStatusId { get; set; }
    }
}
