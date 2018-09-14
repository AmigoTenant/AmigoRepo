using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ProductsMapping: Map<ProductDTO>
    {
        public ProductsMapping()
        {
            PrimaryKey(x => x.ProductId);

            TableName("Product");

            Columns(x =>
            {
                x.Column(y => y.ProductId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.ShortName);
                x.Column(y => y.IsHazardous);
                x.Column(y => y.RowStatus);

            });
        }
    }
}
