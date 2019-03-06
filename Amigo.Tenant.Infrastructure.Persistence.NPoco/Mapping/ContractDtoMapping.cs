using Amigo.Tenant.Application.DTOs.Responses.Expense;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ContractDtoMapping : Map<ContractDTO>
    {
        public ContractDtoMapping()
        {
            TableName("Contract");
        }
    }
}
