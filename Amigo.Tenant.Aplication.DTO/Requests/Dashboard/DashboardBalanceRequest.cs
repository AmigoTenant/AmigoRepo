using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Dashboard
{
    public class DashboardBalanceRequest :PagedRequest
    {
        public int? Frecuency { get; set; }
        public int? PeriodId { get; set; }
        
    }
}
