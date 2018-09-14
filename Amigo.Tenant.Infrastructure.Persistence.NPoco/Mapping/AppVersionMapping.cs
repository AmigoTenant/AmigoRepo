using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{

    public class AppVersionMapping : Map<AppVersionDTO>
    {
        public AppVersionMapping()
        {
            TableName("vwAppVersion");

            Columns(x =>
            {
                x.Column(y => y.AppVersionId);
                x.Column(y => y.VersionNumber);
                x.Column(y => y.Name);

            });
        }
    }

}
