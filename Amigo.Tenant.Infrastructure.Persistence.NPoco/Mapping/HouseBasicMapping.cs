using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.Houses;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class HouseBasicMapping : Map<HouseBasicDTO>
    {
        public HouseBasicMapping()
        {
            TableName("House");
        }
    }
}
