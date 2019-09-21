namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PaymentPeriod")]
    public partial class PaymentPeriod: EntityBase
    {
        public int? PaymentPeriodId { get; set; }
        public int? ConceptId { get; set; }
        public int? ContractId { get; set; }
        public int? TenantId { get; set; }
        public int? PeriodId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DueDate { get; set; }
        public bool RowStatus { get; set; }
        public int? PaymentTypeId { get; set; }
        public string Comment { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ReferenceNo { get; set; }
        public Contract Contract { get; set; }
        public Period Period { get; set; }
        public int? HouseId { get; set; }
    }
}
