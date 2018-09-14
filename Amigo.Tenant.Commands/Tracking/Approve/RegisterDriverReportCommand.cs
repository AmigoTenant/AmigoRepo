using MediatR;
using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;

namespace Amigo.Tenant.Commands.Tracking.Approve
{
   public class RegisterDriverReportCommand : BaseStatusRequest, IAsyncRequest<CommandResult>
    {
        public RegisterDriverReportCommand(int userId)
        {
            CreationDate = DateTime.UtcNow;
            CreatedBy = userId;
            AmigoTenantTServiceIdsListStatus = new List<AmigoTenantTServiceStatus>();
        }

        public RegisterDriverReportCommand(){}

        public int DriverReportId { get; set; }
        public DateTime? ReportDate { get; set; }
        public int? Year { get; set; }
        public int? WeekNumber { get; set; }
        public int? DriverUserId { get; set; }
        public int? ApproverUserId { get; set; }
        public string ApproverSignature { get; set; }
        public decimal? DayPayDriverTotal { get; set; }
        public decimal? DayBillCustomerTotal { get; set; }
        public decimal? TotalHours { get; set; }
        public bool? RowStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<RegisterAmigoTenantTServiceChargeCommand> AmigoTenantTServiceCharges { get; set; }
        public int ActivityTypeId { get; set; }
        public string Username { get; set; }
        public List<AmigoTenantTServiceStatus> AmigoTenantTServiceIdsListStatus { get; set; }

    }

    public class AmigoTenantTServiceStatus
    {
        public string AmigoTenantTServiceId { get; set; }
        public bool? ServiceStatus { get; set; }
        public int? DriverId { get; set; }
    }
}
