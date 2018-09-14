using NPoco.FluentMappings;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class RequestLogMapping : Map<RequestLog>
    {
        public RequestLogMapping()
        {
            PrimaryKey(x => x.RequestLogId);

            TableName("RequestLog");

            Columns(x =>
            {
                x.Column(y => y.RequestLogId);
                x.Column(y => y.Request);
                x.Column(y => y.RequestDate);
                x.Column(y => y.RequestedBy);
                x.Column(y => y.Response);                
                x.Column(y => y.ServiceName);
                x.Column(y => y.URL);                
            });
        }
    }
}