using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Responses.Approve
{
    public class AmigoTenantTServiceApproveRateDTO 
    {
        public string AmigoTenantTServiceId { get; set; }
        public int RateId { get; set; }
        public decimal? PayDriver { get; set; }
        public decimal? BillCustomer { get; set; }
        public string PaidBy { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int IsPerHour { get; set; }
        public int IsPerMove { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeName { get; set; }
        public DateTimeOffset? ServiceStartDate { get; set; }
        public DateTimeOffset? ServiceFinishDate { get; set; }
        public decimal TotalHours { get; set; }
        public string PayBy { get; set; }

    }
}
