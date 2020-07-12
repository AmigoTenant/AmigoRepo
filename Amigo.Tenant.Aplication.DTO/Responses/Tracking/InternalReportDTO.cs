using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{

    public class InternalReportDTO: ExternalReportDTO
    {
        public Decimal DriverPay { get; set; }

    }
}

