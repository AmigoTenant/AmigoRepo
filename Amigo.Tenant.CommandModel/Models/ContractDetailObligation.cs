namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("ContractDetailObligation")]
    public partial class ContractDetailObligation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContractDetailObligation()
        {
            IncomeDetails = new HashSet<IncomeDetail>();
            ContractDetailObligationPays = new HashSet<ContractDetailObligationPay>();
        }

        public int ContractDetailObligationId { get; set; }

        public int? ContractDetailId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ObligationDate { get; set; }

        public int? ConceptId { get; set; }

        [StringLength(150)]
        public string Comment { get; set; }

        public decimal? InfractionAmount { get; set; }

        public int? TenantId { get; set; }

        public int? PeriodId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public int? TenantInfractorId { get; set; }

        public int? EntityStatusId { get; set; }

        public virtual Concept Concept { get; set; }

        public virtual ContractDetail ContractDetail { get; set; }

        public virtual MainTenant Tenant { get; set; }

        public virtual Period Period { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeDetail> IncomeDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligationPay> ContractDetailObligationPays { get; set; }

        public virtual MainTenant TenantInfractor { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }
    }
}
