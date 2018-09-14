using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class DispatchingPartyMapping : Map<DispatchingPartyDTO>
    {
        public DispatchingPartyMapping()
        {
            PrimaryKey(x => x.DispatchingPartyId);

            TableName("DispatchingParty");

            Columns(x =>
            {
                x.Column(y => y.DispatchingPartyId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);
            });
        }


    }
}
