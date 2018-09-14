namespace Amigo.Tenant.CommandModel.Models
{
    using Amigo.Tenant.CommandModel.Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Invoice")]
    public partial class Invoice: EntityBase
    {
        public int? InvoiceId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Comment { get; set; }
        public int? PaymentTypeId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? Taxes { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string CustomerName { get; set; }
        public string PaymentOperationNo { get; set; }
        public string BankName { get; set; }
        public DateTime? PaymentOperationDate { get; set; }
        public bool? RowStatus { get; set; }
        public int? InvoiceStatusId { get; set; }
        public decimal? TotalRent { get; set; }
        public decimal? TotalDeposit { get; set; }
        public decimal? TotalLateFee { get; set; }
        public decimal? TotalService { get; set; }
        public decimal? TotalFine { get; set; }
        public decimal? TotalOnAcount { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
