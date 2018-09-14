namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Income")]
    public partial class Income
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Income()
        {
            IncomeDetails = new HashSet<IncomeDetail>();
        }

        public int IncomeId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? IncomeDate { get; set; }

        [StringLength(20)]
        public string InvoiceNo { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public int? ContractId { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? Tax { get; set; }

        public decimal? SubTotalAmount { get; set; }

        public int? TenantId { get; set; }

        public int? EntityStatusId { get; set; }

        public bool RowStatus { get; set; }
        
        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }
        
        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public int? PeriodId { get; set; }

        public virtual Contract Contract { get; set; }

        public virtual EntityStatus EntityStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeDetail> IncomeDetails { get; set; }

        public virtual MainTenant Tenant { get; set; }

        public virtual Period Period { get; set; }
    }
}
