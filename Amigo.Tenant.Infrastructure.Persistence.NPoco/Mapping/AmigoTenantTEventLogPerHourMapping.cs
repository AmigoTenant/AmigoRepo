using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Approve;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTEventLogPerHourMapping : Map<AmigoTenantTEventLogPerHourDTO>
    {
        public AmigoTenantTEventLogPerHourMapping()
        {
            TableName("vwAmigoTenantTServiceApproveRatesPerHour");

            //Columns(x =>
            //{
            //    x.Column(y => y.DriverUserId).Ignore();
            //    x.Column(y => y.Name);
            //    x.Column(y => y.Code);
            //    x.Column(y => y.Value);
            //    x.Column(y => y.Description);
            //    x.Column(y => y.IsForMobile);
            //    x.Column(y => y.IsForWeb);
            //    x.Column(y => y.RowStatus);
            //});
        }
    }
}
