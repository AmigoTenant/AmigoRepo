using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class FeatureMapping : Map<FeatureDTO>
    {
        public FeatureMapping()
        {
            TableName("vwFeature");

            //Columns(x =>
            //{

            //});
        }
    }
}
