using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTRoleBasicMapping: Map<AmigoTenantTRoleBasicDTO>
    {
        public AmigoTenantTRoleBasicMapping()
        {
            PrimaryKey(x => x.AmigoTenantTRoleId);

            TableName("AmigoTenantTRole");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTRoleId);
                //x.Column(y => y.RowStatus);
            });
        }
    }
}
