using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class ServiceOrderNoRequest
    {
        public int? AmigoTenantTServiceId { get; set; }
        public Guid ServiceOrderNo { get; set; }
    }
}
