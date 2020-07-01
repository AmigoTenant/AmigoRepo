﻿using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractChangeTermRequest : AuditBaseRequest
    {
        public string ContractTermType { get; set; }
        public int? FinalPeriodId { get; set; }
        public int? FromPeriodId { get; set; }

        public int? NewTenantId { get; set; }

        public decimal? NewDeposit { get; set; }
        public decimal? NewRent { get; set; }
        public int? NewHouseId { get; set; }

        public int ContractId { get; set; }
        
    }
}