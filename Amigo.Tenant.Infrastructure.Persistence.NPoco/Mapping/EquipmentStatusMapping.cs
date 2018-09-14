using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
   public class EquipmentStatusMapping : Map<EquipmentStatusDTO>
    {
        public EquipmentStatusMapping()
        {
            PrimaryKey(x => x.EquipmentStatusId);

            TableName("EquipmentStatus");

            Columns(x =>
            {
                x.Column(y => y.EquipmentStatusId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);
            });
        }


    }
}
