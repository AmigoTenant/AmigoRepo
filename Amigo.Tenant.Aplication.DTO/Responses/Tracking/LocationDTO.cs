using System;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class LocationDTO: IEntity
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ZipCode { get; set; }
        public DateTime CreationDate { get; set; }
        public bool HasGeofence { get; set; }
        public string CountryISOCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CityCode { get; set; }
        public string CityName{ get; set; }
        public string LocationTypeCode { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ParentLocationCode { get; set; }
        public string ParentLocationName { get; set; }
        public string LocationTypeName { get; set; }
        public bool RowStatus { get; set; }
    }
}
