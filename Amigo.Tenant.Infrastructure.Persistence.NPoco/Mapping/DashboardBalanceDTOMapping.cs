using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Dashboard;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class DashboardBalanceDTOMapping : Map<DashboardBalanceDTO>
    {
        public DashboardBalanceDTOMapping()
        {
            TableName("vwDashboardBalance");
        }
    }
}
