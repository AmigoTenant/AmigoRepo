namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Feature")]
    public partial class Feature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feature()
        {
            HouseFeatures = new HashSet<HouseFeature>();
        }

        public int FeatureId { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public decimal Measure { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HouseFeature> HouseFeatures { get; set; }

        public bool IsAllHouse { get; set; }

        public int Sequence { get; set; }

        public int HouseTypeId { get; set; }
    }
}
