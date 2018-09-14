namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("House")]
    public partial class House : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public House()
        {
            Contracts = new HashSet<Contract>();
            HouseFeatures = new HashSet<HouseFeature>();
        }

        public int HouseId { get; set; }

        [Required]
        [StringLength(8)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string ShortName { get; set; }

        public int LocationId { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public decimal RentPrice { get; set; }

        public decimal RentDeposit { get; set; }

        public int HouseTypeId { get; set; }

        public int HouseStatusId { get; set; }

        public bool RowStatus { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int? CityId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }

        public virtual GeneralTable GeneralTable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HouseFeature> HouseFeatures { get; set; }

        public ICollection<HouseService> HouseServices { get; set; }
    }
}
