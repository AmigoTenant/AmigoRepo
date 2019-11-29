﻿using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PPHeaderSearchByContractPeriodDTO : AuditBaseRequest
    {
        public int? PaymentPeriodId { get; set; }
        public int? PeriodId { get; set; }
        public string PeriodCode { get; set; }
        public string HouseName { get; set; }
        public string TenantFullName { get; set; }
        public List<PPDetailSearchByContractPeriodDTO> PPDetail { get; set; }
        ObjectStatus TableStatus { get; set; }
        public string Comment { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ContractId { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalRent { get; set; }
        public decimal? TotalDeposit { get; set; }
        public decimal? TotalLateFee { get; set; }
        public decimal? TotalService { get; set; }
        public decimal? TotalFine { get; set; }
        public decimal? TotalOnAcount { get; set; }
        public int? LatestInvoiceId { get; set; }
        public string Email { get; set; }
        public int? TenantId { get; set; }
        public int? PaymentTypeId { get; set; }
        public bool IsPayInFull { get; set; }
        public decimal? TotalInvoice { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? Balance { get; set; }
        public PPDetailSearchByContractPeriodDTO LateFeeMissing { get; set; }
        public int? HouseId { get; set; }
        public bool? IsLiquidating { get; set; }
    }
}
