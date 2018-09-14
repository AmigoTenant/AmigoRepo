using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class EquipmentTypeMapping : Map<EquipmentTypeDTO>
    {

        public EquipmentTypeMapping()
        {
            PrimaryKey(x => x.EquipmentTypeId);

            TableName("EquipmentType");

            Columns(x =>
            {
                x.Column(y => y.EquipmentTypeId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.RowStatus);
                x.Column(y => y.ProductRequiredCode);
                x.Column(y => y.ChassisRequiredCode);
                x.Column(y => y.EquipmentNumberRequiredCode);
            });
        }

    }
}
