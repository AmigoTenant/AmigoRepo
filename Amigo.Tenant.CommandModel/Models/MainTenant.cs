namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Tenant")]
    public partial class MainTenant : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainTenant()
        {
            Contracts = new HashSet<Contract>();
            ContractDetailObligations = new HashSet<ContractDetailObligation>();
            ContractDetailObligations1 = new HashSet<ContractDetailObligation>();
            ContractDetailObligationPays = new HashSet<ContractDetailObligationPay>();
            Incomes = new HashSet<Income>();
            OtherTenants = new HashSet<OtherTenant>();
        }

        [Key]
        public int TenantId { get; set; }

        [StringLength(6)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        public int CountryId { get; set; }

        [StringLength(50)]
        public string PassportNo { get; set; }

        [StringLength(20)]
        public string PhoneN01 { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public bool RowStatus { get; set; }

        //public int? CreatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime? CreationDate { get; set; }

        //public int? UpdatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime? UpdatedDate { get; set; }

        public int? TypeId { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(250)]
        public string Reference { get; set; }

        [StringLength(20)]
        public string PhoneNo2 { get; set; }

        [StringLength(50)]
        public string ContactName { get; set; }

        [StringLength(20)]
        public string ContactPhone { get; set; }

        [StringLength(50)]
        public string ContactEmail { get; set; }

        [StringLength(50)]
        public string ContactRelation { get; set; }

        [StringLength(30)]
        public string IdRef { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contracts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligation> ContractDetailObligations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligation> ContractDetailObligations1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetailObligationPay> ContractDetailObligationPays { get; set; }

        public virtual GeneralTable GeneralTable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income> Incomes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OtherTenant> OtherTenants { get; set; }
    }
}
