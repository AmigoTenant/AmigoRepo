using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ConceptMapping : Map<ConceptDTO>
    {
        public ConceptMapping()
        {
            TableName("vwConcept");

            PrimaryKey(x => x.ConceptId);
            
        }
    }
}
