using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ServiceMapping: Map<ServiceDTO>
    {
        public ServiceMapping()
        {
            PrimaryKey(x => x.ServiceId);

            TableName("vwService");

            Columns(x =>
            {
                x.Column(y => y.ServiceId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.IsPerMove);
                x.Column(y => y.IsPerHour);
                x.Column(y => y.ServiceTypeId);
                x.Column(y => y.RowStatus);
                x.Column(y => y.ServiceTypeCode);
            });
        }
    }
}
