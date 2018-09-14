using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
   
    public class CityMapping : Map<CityDTO>
    {
        public CityMapping()
        {

            TableName("vwCity");

            Columns(x =>
            {
                x.Column(y => y.CityName);
                x.Column(y => y.CityCode);
                x.Column(y => y.StateCode);
                x.Column(y => y.StateName);
                x.Column(y => y.CountryISOCode);
                x.Column(y => y.CountryName);

            });
        }
    }
}
