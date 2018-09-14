using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ActivityTypeMapping : Map<ActivityTypeDTO>
    {
        public ActivityTypeMapping()
        {
            PrimaryKey(x => x.ActivityTypeId);

            TableName("ActivityType");

            Columns(x =>
            {
                x.Column(y => y.ActivityTypeId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);

            });
        }
    }
}
