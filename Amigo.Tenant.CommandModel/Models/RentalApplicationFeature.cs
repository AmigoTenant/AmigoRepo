namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RentalApplicationFeature")]
    public partial class RentalApplicationFeature : EntityBase
    {
        public int? RentalApplicationFeatureId { get; set; }
        public int? RentalApplicationId { get; set; }
        public int? FeatureId { get; set; }

    }
}
