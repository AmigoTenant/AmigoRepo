using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Response.Security;
using Amigo.Tenant.Application.DTOs.Responses.Houses;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class HouseFeatureAndDetailMapping : Map<HouseFeatureAndDetailDTO>
    {
        public HouseFeatureAndDetailMapping()
        {
            TableName("vwHouseFeatureAndDetail");
            Columns(x =>
            {
                x.Column(y => y.IsDisabled).Ignore();
                x.Column(y => y.Marked).Ignore();
                x.Column(y => y.ContractId).Ignore();
                x.Column(y => y.BeginDate).Ignore();
                x.Column(y => y.EndDate).Ignore();
            });
        }
    }
}
