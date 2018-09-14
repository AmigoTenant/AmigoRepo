using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PaymentPeriodByContractMapping : Map<PPSearchByContractPeriodDTO>
    {
        public PaymentPeriodByContractMapping()
        {
            TableName("vwPaymentPeriodByContract");
            Columns(x =>
            {
                x.Column(y => y.IsSelected).Ignore();
                x.Column(y => y.Comment).Ignore();
                x.Column(y => y.PaymentDate).Ignore();
                x.Column(y => y.ReferenceNo).Ignore();
            });
        }
    }
}
