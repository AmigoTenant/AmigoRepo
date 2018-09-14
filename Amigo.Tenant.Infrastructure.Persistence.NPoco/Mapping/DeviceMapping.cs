using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class DeviceMapping : Map<DeviceDTO>
    {
        public DeviceMapping()
        {

            TableName("vwDevice");

            Columns(x =>
            {
                x.Column(y => y.DeviceId);
                x.Column(y => y.CellphoneNumber);
                x.Column(y => y.Identifier);
                x.Column(y => y.WIFIMAC);
                x.Column(y => y.OSVersionId);
                x.Column(y => y.OSVersion);
                x.Column(y => y.OSVersionName);
                x.Column(y => y.PlatformName);
                x.Column(y => y.PlatformId);
                x.Column(y => y.ModelId);
                x.Column(y => y.ModelName);
                x.Column(y => y.BrandId);
                x.Column(y => y.BrandName);
                x.Column(y => y.IsAutoDateTime);
                x.Column(y => y.IsSpoofingGPS);
                x.Column(y => y.IsRootedJailbreaked);
                x.Column(y => y.AppVersionId);
                x.Column(y => y.AppVersion);
                x.Column(y => y.AppVersionName);
                x.Column(y => y.AssignedAmigoTenantTUserId);
                x.Column(y => y.AssignedAmigoTenantTUserUsername);
                x.Column(y => y.RowStatus);
            });
        }
    }
}
