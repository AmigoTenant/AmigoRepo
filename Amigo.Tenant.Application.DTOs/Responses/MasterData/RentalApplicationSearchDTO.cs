using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class RentalApplicationSearchDTO : IEntity
    {
        public int? RentalApplicationId { get; set; }
        public string PeriodCode { get; set; }
        public int? PeriodId { get; set; }
        public int? PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }
        public DateTime? ApplicationDate { get; set; }
        //public string FeaturesCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        //public string Cities { get; set; }
        public int? ResidenseCountryId { get; set; }
        public string ResidenseCountryName { get; set; }
        public string BudgetName { get; set; }
        public int? BudgetId { get; set; }
        public int? AvailableProperties { get; set; }
        public int? RentedProperties { get; set; }

        public int? CityOfInterestId { get; set; }
        public string CityOfInterestName { get; set; }
        public int? HousePartId { get; set; }
        public string HousePartName { get; set; }
        public int? PersonNo { get; set; }
        public int? OutInDownId { get; set; }
        public string OutInDownName { get; set; }
        public int? ReferredById { get; set; }
        public string ReferredByName { get; set; }
        public string ReferredByOther { get; set; }
        //public int? AlertBeforeThat { get; set; }
        public bool? HasNotification { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string AlertMessage { get; set; }


    }
}
