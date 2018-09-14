using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco
{
    public class ParentLocationMapping : Map<ParentLocationDTO>
    {
        public ParentLocationMapping()
        {
            TableName("vwParentLocation");

            Columns(x =>
            {
                x.Column(y => y.Name);
                x.Column(y => y.Code);
            });
        }
    }
}
