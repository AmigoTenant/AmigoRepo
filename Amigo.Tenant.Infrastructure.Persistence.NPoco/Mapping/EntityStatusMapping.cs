using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
   public class EntityStatusMapping : Map<EntityStatusDTO>
    {
        public EntityStatusMapping()
        {
            PrimaryKey(x => x.EntityStatusId);

            TableName("EntityStatus");

            Columns(x =>
            {
            });
        }


    }
}
