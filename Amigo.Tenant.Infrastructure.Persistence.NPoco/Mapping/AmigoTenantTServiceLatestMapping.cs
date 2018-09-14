using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Move;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTServiceLatestMapping: Map<AmigoTenanttServiceLatestDTO>
    {
        public AmigoTenantTServiceLatestMapping()
        {
            //PrimaryKey(x => x.AmigoTenantTServiceId);
            TableName("vwAmigoTenantTServiceLatest");
            //Columns(x =>
            //{
            //    x.Column(y => y.DispatchingPartyId).Ignore();
            //    x.Column(y => y.ServiceStatusOffOnDesc).Ignore();
            //    x.Column(y => y.ServiceStatusOffOnId).Ignore();
             
            //});
        }
    }
}
