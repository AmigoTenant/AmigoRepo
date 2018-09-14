using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class RentalApplicationUpdateRequest : IEntity
    {
        public int? RentalApplicationId { get; set; }
        public int? PeriodId { get; set; }
        public int? PropertyTypeId { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? CityOfInterestId { get; set; }
        public int? HousePartId { get; set; }
        public int? PersonNo { get; set; }
        public int? OutInDownId { get; set; }
        //public List<RentalApplicationCityRequest> Cities { get; set; }
        //public List<RentalApplicationFeatureRequest> Features { get; set; }
        public int? ReferredById { get; set; }
        public string ReferredByOther { get; set; }
        public int? PriorityId { get; set; }
        public DateTime? AlertDate { get; set; }
        public string AlertMessage { get; set; }

    }
}
