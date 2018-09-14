namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Contract")]
    public partial class Contract: EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contract()
        {
            ContractHouseDetails = new HashSet<ContractHouseDetail>();
            ContractDetails = new HashSet<ContractDetail>();
            OtherTenants = new HashSet<OtherTenant>();
            Incomes = new HashSet<Income>();
        }

        public int ContractId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        public decimal RentDeposit { get; set; }

        public decimal RentPrice { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ContractDate { get; set; }

        public int PaymentModeId { get; set; }

        public int ContractStatusId { get; set; }

        public int PeriodId { get; set; }

        [Required]
        [StringLength(11)]
        public string ContractCode { get; set; }

        [StringLength(30)]
        public string ReferencedBy { get; set; }

        public int HouseId { get; set; }

        public bool RowStatus { get; set; }

        [StringLength(18)]
        public string FrecuencyTypeId { get; set; }

        public int? TenantId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractHouseDetail> ContractHouseDetails { get; set; }

        public virtual House House { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractDetail> ContractDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OtherTenant> OtherTenants { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income> Incomes { get; set; }

        public virtual MainTenant Tenant { get; set; }
        

    }
}
