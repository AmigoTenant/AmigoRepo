
namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public  class EquipmentSizeDTO: IEntity
    {
        public int EquipmentSizeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int?  EquipmentTypeId { get; set; }
        public string EquipmentTypeCode { get; set; }
        public bool RowStatus { get; set; }

    }
}
