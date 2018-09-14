using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ContractSearchMapping : Map<ContractSearchDTO>
    {
        public ContractSearchMapping()
        {
            TableName("vwContractSearch");

            Columns(x =>
            {
                x.Column(y => y.IsSelected).Ignore();
            });
        }
    }
}
