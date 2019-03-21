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
            Columns(q => q.Column(r => r.ContractStatusCode).Ignore());
            Columns(q => q.Column(r => r.TenantFullName).Ignore());
            Columns(q => q.Column(r => r.TenantCode).Ignore());
            Columns(q => q.Column(r => r.PeriodCode).Ignore());
            Columns(q => q.Column(r => r.OtherTenantsDTO).Ignore());
        }
    }
}
