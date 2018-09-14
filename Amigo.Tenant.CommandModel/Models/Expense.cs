namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Expense")]
    public partial class Expense : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Expense()
        {
            ExpenseDetails = new HashSet<ExpenseDetail>();
        }

        public int? ExpenseId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ExpenseDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? HouseId { get; set; }
        public int? PeriodId { get; set; }
        public string ReferenceNo { get; set; }
        public string Remark { get; set; }
        public decimal? SubTotalAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpenseDetail> ExpenseDetails { get; set; }
        public bool? RowStatus { get; set; }

    }
}
