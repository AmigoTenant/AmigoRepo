using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class RateMapping : Map<RateDTO>
    {
        public RateMapping()
        {
            TableName("Rate");

            PrimaryKey(x => x.RateId);

            Columns(x =>
            {
                x.Column(y => y.RateId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.PaidBy);
                x.Column(y => y.BillCustomer);
                x.Column(y => y.PayDriver);
            });
        }
    }
}
