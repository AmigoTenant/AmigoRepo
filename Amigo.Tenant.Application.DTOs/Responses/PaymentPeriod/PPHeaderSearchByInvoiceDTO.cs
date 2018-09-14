using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PPHeaderSearchByInvoiceDTO 
    {
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }
        public string PeriodCode { get; set; }
        public string HouseName { get; set; }
        public string TenantFullName { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string PaymentOperationNo { get; set; }
        public string BankName { get; set; }
        public DateTime? PaymentOperationDate { get; set; }
        public string Comment { get; set; }
        public string ContractCode { get; set; }
        public Decimal? TotalRent { get; set; }
        public Decimal? TotalDeposit { get; set; }
        public Decimal? TotalLateFee { get; set; }
        public Decimal? TotalService { get; set; }
        public Decimal? TotalFine { get; set; }
        public Decimal? TotalOnAcount { get; set; }
        public Decimal? TotalAmount { get; set; }
        public int? ContractId { get; set; }
        public int? PaymentPeriodId { get; set; }
        public string PaymentTypeValue { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentDescription { get; set; }
        public Decimal? PaymentAmount { get; set; }
        public DateTime? PeriodDueDate { get; set; }
        public int? PaymentTypeSequence { get; set; }

    }
}
