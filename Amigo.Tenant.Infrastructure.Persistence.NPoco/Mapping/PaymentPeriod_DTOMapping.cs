using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PaymentPeriod_DTOMapping : Map<PaymentPeriodDTO>
    {
        public PaymentPeriod_DTOMapping()
        {
            TableName("PaymentPeriod");
            Columns(x =>
            {
                //x.Column(y => y.IsSelected).Ignore();
                //x.Column(y => y.Code);
            });
        }
    }
}
