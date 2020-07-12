using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Dashboard
{
    public class DashboardBalanceDTO 
    {
        public int? PeriodCode { get; set; }
        public int? Anio { get; set; }
        public decimal? TotalIncomePaidAmount { get; set; }
        public decimal? TotalExpenseAmount { get; set; }
        public decimal? TotalIncomePendingAmount { get; set; }
    }
}
