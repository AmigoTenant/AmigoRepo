using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class EquipmentSizeMapping : Map<EquipmentSizeDTO>
    {
        public EquipmentSizeMapping()
        {
            PrimaryKey(x => x.EquipmentSizeId);

            TableName("vwEquipmentSize");

            Columns(x =>
            {
                x.Column(y => y.EquipmentSizeId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.EquipmentTypeId);
                x.Column(y => y.RowStatus);
                x.Column(y => y.EquipmentTypeCode);
            });
        }


    }
}
