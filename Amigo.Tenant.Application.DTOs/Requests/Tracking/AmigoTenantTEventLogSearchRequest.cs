using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class AmigoTenantTEventLogSearchRequest : PagedRequest
    {
        public string  ActivityTypeCode { get; set; }
        public string Username { get; set; }
        public DateTimeOffset? ReportedActivityDateFrom { get; set; }
        public DateTimeOffset? ReportedActivityDateTo { get; set; }

        public string ReportedActivityTimeZone { get; set; }

        public string LogType { get; set; }
        public string Parameters { get; set; }
        public int? AmigoTenantMoveId { get; set; }
        public string AmigoTenantMoveNumber { get; set; }
        public string ShipmentID { get; set; }
        public string CostCenterCode { get; set; }
        public int? CostCenterId { get; set; }
        public string EquipmentNumber { get; set; }
        public int? EquipmentId { get; set; }
        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }
        public string Platform { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
     
        public string LocationProvider { get; set; }

    }

}
