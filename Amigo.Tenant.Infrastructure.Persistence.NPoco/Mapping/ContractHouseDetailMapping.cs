using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ContractHouseDetailMapping : Map<ContractHouseDetailDTO>
    {
        public ContractHouseDetailMapping()
        {
            TableName("vwContractHouseDetail");

            Columns(x =>
            {
            });
        }
    }
}
