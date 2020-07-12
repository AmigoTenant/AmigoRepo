using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class AmigoTenantTServiceSearchChargeNumRequest
    {
        public string ChargeNumber { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
