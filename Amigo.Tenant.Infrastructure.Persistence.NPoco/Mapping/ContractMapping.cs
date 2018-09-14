using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ContractMapping : Map<ContractRegisterRequest>
    {
        public ContractMapping()
        {
            TableName("vwContract");

            Columns(x =>
            {
                x.Column(y => y.OtherTenants).Ignore();
                x.Column(y => y.ContractDetails).Ignore();
                x.Column(y => y.ContractHouseDetails).Ignore();
                x.Column(y => y.UserId).Ignore();
                x.Column(y => y.Username).Ignore();
                x.Column(y => y.PaymentsPeriod).Ignore();
            });
        }
    }
}
