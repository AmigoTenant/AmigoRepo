using NPoco.FluentMappings;
using XPO.ShuttleTracking.Application.DTOs.Responses.Security;

namespace XPO.ShuttleTracking.Infrastructure.Persistence.NPoco.Mapping
{
    public class RolesMapping: Map<RoleDTO>
    {
        public RolesMapping()
        {
            PrimaryKey(x => x.RoleId);

            TableName("Role");

            Columns(x =>
            {
                x.Column(y => y.RoleId);
                x.Column(y => y.Name);
                x.Column(y => y.Code);
            });
        }
    }
}
