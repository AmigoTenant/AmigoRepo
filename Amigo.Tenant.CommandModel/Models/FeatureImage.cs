namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("FeatureImage")]
    public partial class FeatureImage
    {
        public int FeatureImageId { get; set; }

        public int HouseFeatureId { get; set; }

        [Required]
        [StringLength(250)]
        public string ImagePath { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual HouseFeature HouseFeature { get; set; }
    }
}
