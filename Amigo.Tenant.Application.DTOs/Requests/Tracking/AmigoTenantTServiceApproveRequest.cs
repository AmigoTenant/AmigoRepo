using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class AmigoTenantTServiceApproveRequest: AuditBaseRequest
    {
        public DateTime? ReportDate { get; set; }
        public int DriverId { get; set; }
        public DateTime? CurrentTime{get; set; }
        public string ApprovedBy { get; set; }
        public List<AmigoTenantTServiceStatus> AmigoTenantTServiceIdsListStatus { get; set; }
        public string ApprovalComments { get; set; }
        public string DriverName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsApprove { get; set; }
        public string MoveOrHour { get; set; }
    }

    public class AmigoTenantTServiceStatus
    {
        public string   AmigoTenantTServiceId { get; set; }
        public bool? ServiceStatus { get; set; }
        public int? DriverId { get; set; }
    }

}
