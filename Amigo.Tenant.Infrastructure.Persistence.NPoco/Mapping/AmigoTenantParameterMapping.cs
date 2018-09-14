using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantParameterMapping : Map<AmigoTenantParameterDTO>
    {

        public AmigoTenantParameterMapping()
        {
            PrimaryKey(x => x.AmigoTenantParameterId);

            TableName("AmigoTenantParameter");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantParameterId);
                x.Column(y => y.Name);
                x.Column(y => y.Code);
                x.Column(y => y.Value);
                x.Column(y => y.Description);
                x.Column(y => y.IsForMobile);
                x.Column(y => y.IsForWeb);
                x.Column(y => y.RowStatus);
            });
        }

    }
}
