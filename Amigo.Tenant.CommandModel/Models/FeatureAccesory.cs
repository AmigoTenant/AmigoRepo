namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("FeatureAccesory")]
    public partial class FeatureAccesory
    {
        public int FeatureAccesoryId { get; set; }

        public int AccesoryId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public int? HouseFeatureId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual GeneralTable GeneralTable { get; set; }

        public virtual HouseFeature HouseFeature { get; set; }
    }
}
