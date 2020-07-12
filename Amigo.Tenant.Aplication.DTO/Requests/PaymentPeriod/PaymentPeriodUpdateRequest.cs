using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodUpdateRequest: AuditBaseRequest
    {
        public int PaymentPeriodId { get; set; }
        public decimal? PaymentAmount{ get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Comment { get; set; }
        public string ReferenceNo { get; set; }
    }
}
