using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Move;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTServiceReportMapping: Map<AmigoTenantTServiceReportDTO>
    {
        public AmigoTenantTServiceReportMapping()
        {
            PrimaryKey(x => x.AmigoTenantTServiceId);

            TableName("vwAmigoTenantTService");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTServiceId);
                x.Column(y => y.UserName);
                x.Column(y => y.IsSelected).Ignore();
                x.Column(y => y.ChargeNoType).Ignore();
                x.Column(y => y.ActivityCode).Ignore();
                x.Column(y => y.ShipmentID).Ignore();
                x.Column(y => y.CostCenterCode).Ignore();
                x.Column(y => y.ChargeNo);
                x.Column(y => y.DriverComments);
                x.Column(y => y.ApprovalComments); 
            });
        }
    }
}
