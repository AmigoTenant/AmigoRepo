using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class RentalApplicationSearchRequest : PagedRequest
    {
        public int? PeriodId { get; set; }
        public int? PropertyTypeId { get; set; }
        public DateTime? ApplicationDateFrom { get; set; }
        public DateTime? ApplicationDateTo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? CheckInFrom { get; set; }
        public DateTime? CheckInTo { get; set; }
        public DateTime? CheckOutFrom { get; set; }
        public DateTime? CheckOutTo { get; set; }
        public int? ResidenseCountryId { get; set; }
        public string ResidenseCountryName { get; set; }
        public int? BudgetId { get; set; }
        public List<int?> FeatureIds  { get; set; }
        public List<int?> CitiesIds { get; set; }
        public int? CityOfInterestId { get; set; }
        public int? HousePartId { get; set; }
        public int? PersonNo { get; set; }
        public int? OutInDownId { get; set; }
        public int? ReferredById { get; set; }


    }
}
