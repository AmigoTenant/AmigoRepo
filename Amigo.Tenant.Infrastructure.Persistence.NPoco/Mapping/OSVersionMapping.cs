using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{

    public class OSVersionMapping : Map<OSVersionDTO>
    {
        public OSVersionMapping()
        {
            TableName("vwOSVersion");

            Columns(x =>
            {
                x.Column(y => y.PlatformId);
                x.Column(y => y.PlatformName);
                x.Column(y => y.OSVersionId);
                x.Column(y => y.VersionNumber);
                x.Column(y => y.VersionName);
            });
        }
    }

}
