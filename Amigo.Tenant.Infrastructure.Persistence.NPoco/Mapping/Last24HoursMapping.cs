using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{

    public class Last24HoursMapping : Map<Last24HoursDTO>
    {
        public Last24HoursMapping()
        {

            TableName("vwLast24Hours");

            Columns(x =>
            {

                x.Column(y => y.AmigoTenantTUserId);
                x.Column(y => y.Username);
                x.Column(y => y.ReportedActivityTimeZone);
                x.Column(y => y.ReportedActivityDate);
                x.Column(y => y.Latitude);
                x.Column(y => y.Longitude);
                x.Column(y => y.ChargeType);
                x.Column(y => y.ActivityTypeName);
                x.Column(y => y.ActivityTypeCode);
                x.Column(y => y.TractorNumber);
                x.Column(y => y.FirstName).Ignore();
                x.Column(y => y.LastName).Ignore();
                x.Column(y => y.ChassisNumber);
                x.Column(y => y.EquipmentNumber);
                x.Column(y => y.ChargeNo);
                x.Column(y => y.Product);
                x.Column(y => y.Origin);
                x.Column(y => y.Destination);
                x.Column(y => y.Index).Ignore();
                x.Column(y => y.ServiceName);
                x.Column(y => y.EquipmentTypeName);

            });
        }
    }


}

