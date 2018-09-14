using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodSearchByContractPeriodRequest : PagedRequest
    {
        public int? ContractId { get; set; }
        public int? PeriodId { get; set; }

    }
}
