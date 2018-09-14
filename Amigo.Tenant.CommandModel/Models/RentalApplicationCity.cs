namespace Amigo.Tenant.CommandModel.Models
{
    using Abstract;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RentalApplicationCity")]
    public partial class RentalApplicationCity 
    {
        public int? RentalApplicationCityId { get; set; }
        public int? RentalApplicationId { get; set; }
        public int? CityId { get; set; }

    }
}
