using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{


    public class LatestPositionMapping : Map<LatestPositionDTO>
    {
        public LatestPositionMapping()
        {

            TableName("vwLatestPosition");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTEventLogId);
                x.Column(y => y.ActivityTypeName);
                x.Column(y => y.ActivityTypeCode);
                x.Column(y => y.Username);
                x.Column(y => y.AmigoTenantTUserId);
                x.Column(y => y.ReportedActivityDate);
                x.Column(y => y.ReportedActivityTimeZone);
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.TractorNumber);
                x.Column(y => y.FirstName);
                x.Column(y => y.LastName);
                x.Column(y => y.ChargeNo);

            });
        }
    }

}
