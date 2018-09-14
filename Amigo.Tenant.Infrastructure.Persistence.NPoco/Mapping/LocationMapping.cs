using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class LocationMapping : Map<LocationDTO>
    {
        public LocationMapping()
        {

            TableName("vwLocation");

            Columns(x =>
            {
                x.Column(y => y.LocationId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.Address1);
                x.Column(y => y.Address2);
                x.Column(y => y.ZipCode);
                x.Column(y => y.CreationDate);
                x.Column(y => y.ParentLocationCode);
                x.Column(y => y.ParentLocationName);
                x.Column(y => y.LocationTypeName);
                x.Column(y => y.LocationTypeCode);
                x.Column(y => y.CityCode);
                x.Column(y => y.CityName);
                x.Column(y => y.StateName);
                x.Column(y => y.StateCode);
                x.Column(y => y.CountryName);
                x.Column(y => y.CountryISOCode);
                x.Column(y => y.RowStatus);

            });
        }
    }
}
