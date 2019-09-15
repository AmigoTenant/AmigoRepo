using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractChangeTermRequest : AuditBaseRequest
    {
        public int? PeriodId { get; set; }
        public int? ContractId { get; set; }
        public int? TenantId { get; set; }
        public int? HouseId { get; set; }
    }
}
