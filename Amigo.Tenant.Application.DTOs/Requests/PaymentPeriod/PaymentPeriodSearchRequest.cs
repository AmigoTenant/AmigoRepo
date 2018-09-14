using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodSearchRequest :PagedRequest
    {
        public int? PeriodId { get; set; }
        public int? HouseId { get; set; }
        public string ContractCode { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public int? TenantId { get; set; }
        public bool? HasPendingServices { get; set; }
        public bool? HasPendingFines { get; set; }
        public bool? HasPendingLateFee { get; set; }
        public bool? HasPendingDeposit { get; set; }
    }
}
