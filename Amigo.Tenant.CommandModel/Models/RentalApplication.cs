namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RentalApplication")]
    public partial class RentalApplication : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RentalApplication()
        {
            RentalApplicationFeatures = new HashSet<RentalApplicationFeature>();
            RentalApplicationCities = new HashSet<RentalApplicationCity>();
        }

        public int? RentalApplicationId { get; set; }
        public int? PeriodId { get; set; }
        public int? PropertyTypeId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ApplicationDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HousePhone { get; set; }
        public string CellPhone { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? ResidenseCountryId { get; set; }
        public int? BudgetId { get; set; }
        public string Comment { get; set; }
        public bool? RowStatus { get; set; }
        public int? CityOfInterestId { get; set; }
        public int? HousePartId { get; set; }
        public int? PersonNo { get; set; }
        public int? OutInDownId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentalApplicationFeature> RentalApplicationFeatures { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentalApplicationCity> RentalApplicationCities { get; set; }
        public int? ReferredById { get; set; }
        public string ReferredByOther { get; set; }
        //public int? AlertBeforeThat { get; set; }
        public int? PriorityId { get; set; }
        public DateTime? AlertDate { get; set; }
        public string AlertMessage { get; set; }


    }
}
