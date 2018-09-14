namespace Amigo.Tenant.CommandModel.Models
{
    using Amigo.Tenant.CommandModel.Abstract;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExpenseDetail")]
    public partial class ExpenseDetail : EntityBase
    {
        public int? ExpenseDetailId { get; set; }
        public int? ExpenseId { get; set; }
        public int? ConceptId { get; set; }
        [StringLength(250)]
        public string Remark { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? TenantId { get; set; }
        public int? ExpenseDetailStatusId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual Concept Concept { get; set; }
        public virtual MainTenant Tenant { get; set; }
    }
}
