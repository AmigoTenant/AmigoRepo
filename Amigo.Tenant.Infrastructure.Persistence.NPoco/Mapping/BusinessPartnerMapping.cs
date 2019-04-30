using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class BusinessPartnerDtoMapping : Map<BusinessPartnerDTO>
    {
        public BusinessPartnerDtoMapping()
        {
            TableName("vwBusinessPartner");

        }
    }
}
