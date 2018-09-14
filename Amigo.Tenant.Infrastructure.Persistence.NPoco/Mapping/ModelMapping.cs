using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{

    public class ModelMapping : Map<ModelDTO>
    {
        public ModelMapping()
        {
            TableName("vwModel");

            Columns(x =>
            {
                x.Column(y => y.BrandId);
                x.Column(y => y.BrandName);
                x.Column(y => y.ModelId);
                x.Column(y => y.ModelName);
            });
        }
    }
}
