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
            Columns(x =>
            {
                //x.Column(y => y.IsSelected).Ignore();
                //x.Column(y => y.FinesAmountPending).Ignore();
                //x.Column(y => y.DepositAmountPending).Ignore();
                //x.Column(y => y.LateFeesAmountPending).Ignore();
                ////x.Column(y => y.OnAccountAmountPending).Ignore();
                //x.Column(y => y.PaymentAmount).Ignore();
                //x.Column(y => y.ServicesAmountPending).Ignore();
                //x.Column(y => y.TotalAmountPending).Ignore();
                //x.Column(y => y.Code);
            });
        }
    }
}
