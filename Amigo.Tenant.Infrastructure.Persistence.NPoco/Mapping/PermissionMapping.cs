using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PermissionMapping: Map<PermissionDTO>
    {
        public PermissionMapping()
        {
            PrimaryKey(x => x.PermissionId);

            TableName("vwPermission");

            Columns(x =>
            {
                x.Column(y => y.PermissionId);
                x.Column(y => y.AmigoTenantTRoleId);
                x.Column(y => y.ActionCode).WithName("Code");
                x.Column(y => y.ActionId);
                x.Column(y => y.TableStatus).Ignore();
                x.Column(y => y.AmigoTenantTRoleCode).Ignore();
            });
        }
    }
}
