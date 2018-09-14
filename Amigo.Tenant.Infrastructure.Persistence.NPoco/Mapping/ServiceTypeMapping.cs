using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ServiceTypeMapping : Map<ServiceTypeDTO>
    {
        public ServiceTypeMapping()
        {
            PrimaryKey(x => x.ServiceTypeId);

            TableName("ServiceType");

            Columns(x =>
            {
                x.Column(y => y.ServiceTypeId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);

            });
        }
    }
}
