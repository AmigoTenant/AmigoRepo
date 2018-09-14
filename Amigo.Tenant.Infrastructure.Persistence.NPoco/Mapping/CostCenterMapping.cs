using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
   public class CostCenterMapping : Map<CostCenterDTO>
    {
        public CostCenterMapping()
        {
            PrimaryKey(x => x.CostCenterId);

            TableName("CostCenter");

            Columns(x =>
            {
                x.Column(y => y.CostCenterId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);
            });
        }


    }
}
