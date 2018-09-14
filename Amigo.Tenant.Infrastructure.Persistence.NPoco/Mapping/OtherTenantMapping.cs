using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class OtherTenantMapping : Map<OtherTenantRegisterRequest>
    {
        public OtherTenantMapping()
        {
            TableName("vwOtherTenant");

            Columns(x =>
            {
                //x.Column(y => y.TableStatus).Ignore();
                //x.Column(y => y.RowStatus).Ignore();
                //x.Column(y => y.CreatedBy).Ignore();
                //x.Column(y => y.CreationDate).Ignore();
                //x.Column(y => y.UpdatedBy).Ignore();
                //x.Column(y => y.UpdatedDate).Ignore();
            });
        }
    }
}
