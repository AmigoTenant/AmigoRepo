using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class DriverPayReportDTO //MobileRequestBase /*, BaseDTO*/
    {
        public DateTime? ReportDate { get; set; }
        public decimal? DayPayDriverTotal { get; set; }
        public int? CurrentLocationId { get; set; }
        public int? DedicatedLocationId { get; set; }

        /*OTHERS*/
        public int? DriverUserId { get; set; }
        public int? DispatcherPartyId { get; set; }
        public string Driver { get; set; }
        public string ServiceStatusOffOnDesc { get; set; }
        public string DispatcherCode { get; set; }
        public string CurrentLocationCode { get; set; }
        public string DedicatedLocationCode { get; set; }
        public int? ServiceStatusOffOnId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ServiceLatestInformation { get; set; }
        public string ChargeNo { get; set; }
        public string ChargeType { get; set; }
    }
}
