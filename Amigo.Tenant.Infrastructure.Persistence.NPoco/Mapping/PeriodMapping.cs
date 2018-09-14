using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PeriodMapping : Map<PeriodDTO>
    {
        public PeriodMapping()
        {
            TableName("Period");
        }
    }
}
