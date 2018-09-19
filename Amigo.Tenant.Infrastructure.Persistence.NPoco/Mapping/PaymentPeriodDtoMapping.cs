using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PaymentPeriodGroupedStatusAndConceptDTOMapping : Map<PaymentPeriodGroupedStatusAndConceptDTO>
    {
        public PaymentPeriodGroupedStatusAndConceptDTOMapping()
        {
            TableName("vwPaymentPeriodGroupedStatusAndConcept");
            Columns(x =>
            {
                //x.Column(y => y.IsSelected).Ignore();
                //x.Column(y => y.Code);
            });
        }
    }
}
