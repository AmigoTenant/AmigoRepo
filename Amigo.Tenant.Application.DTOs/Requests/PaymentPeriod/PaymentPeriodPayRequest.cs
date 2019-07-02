using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodPayRequest: AuditBaseRequest
    {
        public int PaymentPeriodId { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
