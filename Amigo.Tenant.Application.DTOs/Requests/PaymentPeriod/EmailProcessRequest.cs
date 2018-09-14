using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class EmailProcessRequest
    {
        public int? PeriodId { get; set; }
        public int? TenantId { get; set; }

    }
}
