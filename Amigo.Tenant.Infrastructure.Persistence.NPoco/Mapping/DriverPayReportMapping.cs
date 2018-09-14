using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class DriverPayReportMapping : Map<DriverPayReportDTO>
    {
        public DriverPayReportMapping()
        {
            TableName("vwDriverPayReport");
            Columns(x =>
            {
                x.Column(y => y.ServiceStatusOffOnDesc).Ignore();
                x.Column(y => y.DispatcherPartyId).Ignore();
                x.Column(y => y.DispatcherCode).Ignore();
                x.Column(y => y.CurrentLocationCode).Ignore();
                x.Column(y => y.CurrentLocationId).Ignore();
                x.Column(y => y.ServiceStatusOffOnId).Ignore();
                x.Column(y => y.ChargeNo).Ignore();
                x.Column(y => y.ChargeType).Ignore();
                x.Column(y => y.FirstName).Ignore();
                x.Column(y => y.LastName).Ignore();
                x.Column(y => y.ServiceLatestInformation).Ignore();
                
            });
        }
    }
}
