using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class PaymentPeriodPrintMapping : Map<PPHeaderSearchByInvoiceDTO>
    {
        public PaymentPeriodPrintMapping()
        {
            TableName("vwPaymentPeriodInvoicePrint");
        }
    }
}
