using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractSearchRequest :PagedRequest
    {
        public int? PeriodId { get; set; }
        public string ContractCode { get; set; }
        public int? ContractStatusId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TenantFullName { get; set; }
        public int? HouseId { get; set; }
        //public int? FeatureId { get; set; }
        public string UnpaidPeriods { get; set; }
        public int? NextDaysToCollect { get; set; }
        //public DateTime? NextPeriodDate { get; set; }
        //public DateTime? NextDueDate { get; set; }
        public List<int?> FeatureIds  { get; set; }

    }
}
