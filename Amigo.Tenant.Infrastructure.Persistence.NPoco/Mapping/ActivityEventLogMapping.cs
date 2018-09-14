using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ActivityEventLogMapping : Map<ActivityEventLogDTO>
    {
        public ActivityEventLogMapping()
        {

            TableName("vwActivityEventLog");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTEventLogId);
                x.Column(y => y.ActivityTypeId);
                x.Column(y => y.ActivityName);
                x.Column(y => y.Username);
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.ReportedActivityDate);
                x.Column(y => y.LocationProvider);
                x.Column(y => y.TableStatus).Ignore();
                x.Column(y => y.OriginLocationName);
                x.Column(y => y.OriginLocationId);
                x.Column(y => y.DestinationLocationId);
                x.Column(y => y.DestinationLocationName);
                x.Column(y => y.EquipmentNumber);
                x.Column(y => y.ProductName); 
                x.Column(y => y.ChargeNo);
                x.Column(y => y.Parameters);
                x.Column(y => y.LogType);
            });
        }
    }
}
