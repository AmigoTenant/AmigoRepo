using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    
    public class LocationSearchRequest : PagedRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ZipCode { get; set; }
        public bool? HasGeofence { get; set; }
        public string CountryISOCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string LocationTypeCode { get; set; }
        public string ParentLocationCode { get; set; }


    }
}
