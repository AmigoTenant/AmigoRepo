namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("ContractDetail")]
    public partial class ContractDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContractDetail()
        {
            IncomeDetails = new HashSet<IncomeDetail>();
            ContractDetailObligations = new HashSet<ContractDetailObligation>();
        }

        public int ContractDetailId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DueDate { get; set; }

        public int ItemNo { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Comment { get; set; }

        public decimal Rent { get; set; }

        public int ContractId { get; set; }

        public int ContractDetailStatusId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PaymentDate { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public int? PeriodId { get; set; }

        public int? DelayDays { get; set; }

        public decimal? FinePerDay { get; set; }

        public decimal? FineAmount { get; set; }

        public decimal? TotalPayment { get; set; }

        public int? PayTypeId { get; set; }

        [StringLength(50)]
        public string PaymentReferenceNo { get; set; }

        public virtual Contract Contract { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeDetail> IncomeDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligation> ContractDetailObligations { get; set; }

        public virtual Period Period { get; set; }

        public virtual GeneralTable GeneralTable { get; set; }
    }
}
