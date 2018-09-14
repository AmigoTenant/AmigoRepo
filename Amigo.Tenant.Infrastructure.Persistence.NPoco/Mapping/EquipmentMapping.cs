using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class EquipmentMapping : Map<EquipmentDTO>
    {
        public EquipmentMapping()
        {
            PrimaryKey(x => x.EquipmentId);

            TableName("vwEquipment");

            Columns(x =>
            {
                x.Column(y => y.EquipmentSizeId);
                x.Column(y => y.EquipmentNo);
                x.Column(y => y.EquipmentSizeCode);
                x.Column(y => y.EquipmentSizeName);
                x.Column(y => y.EquipmentTypeCode);
                x.Column(y => y.EquipmentTypeName);
                x.Column(y => y.RowStatus);
            });
        }


    }
}
