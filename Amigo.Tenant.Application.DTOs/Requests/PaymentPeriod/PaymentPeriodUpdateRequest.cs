using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodUpdateRequest: AuditBaseRequest
    {
        public int PaymentPeriodId { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
