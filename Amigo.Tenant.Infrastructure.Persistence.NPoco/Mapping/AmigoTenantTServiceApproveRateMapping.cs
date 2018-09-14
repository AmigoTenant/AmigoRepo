using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Move;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTServiceApproveRateMapping : Map<AmigoTenantTServiceApproveRateDTO>
    {
        public AmigoTenantTServiceApproveRateMapping()
        {
            PrimaryKey(x => x.AmigoTenantTServiceId);

            TableName("vwAmigoTenantTServiceApproveRates");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTServiceId);
            });
        }
    }
}
