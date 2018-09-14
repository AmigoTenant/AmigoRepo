namespace Amigo.Tenant.CommandModel.Models
{
    using Amigo.Tenant.CommandModel.Abstract;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InvoiceDetail")]
    public partial class InvoiceDetail : EntityBase
    {
        public int? InvoiceDetailId { get; set; }
        public int? InvoiceId { get; set; }
        public int? PaymentPeriodId { get; set; }
        public int? ConceptId { get; set; }
        public int? Qty { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? RowStatus { get; set; }
        public PaymentPeriod PaymentPeriod { get; set; }

    }
}
