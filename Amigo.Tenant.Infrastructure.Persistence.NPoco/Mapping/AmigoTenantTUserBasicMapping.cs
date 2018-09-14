using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTUserBasicMapping: Map<AmigoTenantTUserBasicDTO>
    {
        public AmigoTenantTUserBasicMapping()
        {
            PrimaryKey(x => x.AmigoTenantTUserId);

            TableName("vwAmigoTenantTUser");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTUserId);
                x.Column(y => y.TableStatus).Ignore();
                x.Column(y => y.FirstName);
                x.Column(y => y.LastName);
                x.Column(y => y.CustomUsername).Ignore();
            });
        }
    }
}
