using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTEventLogSearchResultMapping : Map<AmigoTenantTEventLogSearchResultDTO>
    {
        public AmigoTenantTEventLogSearchResultMapping()
        {
            PrimaryKey(x => x.AmigoTenantTEventLogId);

            TableName("vwAmigoTenantTEventLog");

            Columns(x =>
            {

                x.Column(y => y.AmigoTenantTEventLogId);
                x.Column(y => y.ActivityTypeId);
                x.Column(y => y.ActivityTypeCode);
                x.Column(y => y.ActivityTypeName);
                x.Column(y => y.Username);
                x.Column(y => y.ReportedActivityDate);
                x.Column(y => y.ReportedActivityTimeZone);
                x.Column(y => y.ConvertedActivityUTC);
                x.Column(y => y.LogType);
                x.Column(y => y.Parameters);
                x.Column(y => y.AmigoTenantMoveId);
                x.Column(y => y.AmigoTenantMoveNumber);
                x.Column(y => y.EquipmentNumber);
                x.Column(y => y.EquipmentId);
                x.Column(y => y.IsAutoDateTime);
                x.Column(y => y.IsSpoofingGPS);
                x.Column(y => y.IsRootedJailbreaked);
                x.Column(y => y.Platform);
                x.Column(y => y.OSVersion);
                x.Column(y => y.AppVersion);
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.Accuracy);
                x.Column(y => y.LocationProvider);
                x.Column(y => y.CreatedBy);
                x.Column(y => y.CreationDate);
                x.Column(y => y.UpdatedBy);
                x.Column(y => y.UpdatedDate);
                x.Column(y => y.RowStatus);
                x.Column(y => y.ChargeNo);
            });
        }
    }
}
