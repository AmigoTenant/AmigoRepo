using System;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class AmigoTenantTServiceApproveSearchRequest
    {
        public DateTimeOffset? ServiceDate
        {
            get; set;
        }

        public DateTime? ReportDateFrom
        {
            get; set;
        }
        public DateTime? ReportDateTo
        {
            get; set;
        }

        public string PaidBy
        {
            get; set;
        }

        public int? DriverId
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        public int? ServiceStatusId
        {
            get; set;
        }

        public string ApprovedBy
        {
            get; set;
        }

        public string ApprovalComments
        {
            get; set;
        }

        public int Page
        {
            get; set;
        }
        public int PageSize
        {
            get; set;
        }
    }
}
