using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;


namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
   public class DriverPayReportSearchRequest : BaseStatusRequest
    {
        public int? DriverId { get; set; }
        public int? ServiceStatusOffOnId { get; set; }
        public string DispatchingPartyCode { get; set; }
        public int? CurrentLocationId { get; set; }
        public int? DedicatedLocationId { get; set; }
        public DateTime? ReportDateFrom { get; set; }
        public DateTime? ReportDateTo { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
