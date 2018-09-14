using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Move;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTServiceMapping: Map<AmigoTenanttServiceDTO>
    {
        public AmigoTenantTServiceMapping()
        {
            PrimaryKey(x => x.AmigoTenantTServiceId);
            TableName("AmigoTenantTService");
            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTServiceId);
                x.Column(y => y.ActivityTypeId).Ignore();
                x.Column(y => y.IsAutoDateTime).Ignore();
                x.Column(y => y.IsSpoofingGPS).Ignore();
                x.Column(y => y.IsRootedJailbreaked).Ignore();
                x.Column(y => y.Platform).Ignore();
                x.Column(y => y.OSVersion).Ignore();
                x.Column(y => y.AppVersion).Ignore();
                x.Column(y => y.Latitude).Ignore();
                x.Column(y => y.Longitude).Ignore();
                x.Column(y => y.Accuracy).Ignore();
                x.Column(y => y.LocationProvider).Ignore();
                x.Column(y => y.UserId).Ignore();
                x.Column(y => y.ReportedActivityDate).Ignore();
                x.Column(y => y.ReportedActivityTimeZone).Ignore();
                x.Column(y => y.IncludeRequestLog).Ignore();
                x.Column(y => y.CostCenterId).Ignore();
                x.Column(y => y.ShipmentID).Ignore();
                x.Column(y => y.Username).Ignore();
                x.Column(y => y.ChargeNo);
            });
        }
    }
}
