using MediatR;
using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;

namespace Amigo.Tenant.Commands.Tracking.Approve
{
   public class RegisterAmigoTenantTServiceChargeCommand : BaseStatusRequest, IAsyncRequest<CommandResult>
    {
        public int AmigoTenantTServiceChargeId { get; set; }
        public decimal? DriverPay { get; set; }
        public decimal? CustomerBill { get; set; }
        public int? AmigoTenantTServiceId { get; set; }
        public int? RateId { get; set; }
        public int? DriverReportId { get; set; }
        public UpdateAmigoTenantServiceCommand AmigoTenantTService { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? TotalHours { get; set; }
        public string PayBy { get; set; }
        public List<UpdateAmigoTenantServiceCommand> AmigoTenantTServiceList { get; set; }
        public bool? ApproveOrReject { get; set; }

    }
}
