using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.Houses;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class HouseFeatureDetailContractMapping : Map<HouseFeatureDetailContractDTO>
    {
        public HouseFeatureDetailContractMapping()
        {
            TableName("vwHouseFeatureDetailContract");
            //Columns(x =>
            //{
            //    x.Column(y => y.IsDisabled).Ignore();
            //});
        }
    }
}
