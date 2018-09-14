using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{

    public class LocationTypeMapping : Map<LocationTypeDTO>
    {
        public LocationTypeMapping()
        {

            TableName("vwLocationType");

            Columns(x =>
            {
                x.Column(y => y.Name);
                x.Column(y => y.Code);

            });
        }
    }

}
