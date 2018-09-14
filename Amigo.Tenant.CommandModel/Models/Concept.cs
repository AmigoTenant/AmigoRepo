namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Concept")]
    public partial class Concept
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Concept()
        {
            IncomeDetails = new HashSet<IncomeDetail>();
            ContractDetailObligations = new HashSet<ContractDetailObligation>();
        }

        public int ConceptId { get; set; }

        [Required]
        [StringLength(8)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int TypeId { get; set; }

        public bool? RowStatus { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int PayTypeId { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public decimal? ConceptAmount { get; set; }
        
        public virtual GeneralTable GeneralTable { get; set; }

        public virtual GeneralTable GeneralTable1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeDetail> IncomeDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligation> ContractDetailObligations { get; set; }
    }
}
