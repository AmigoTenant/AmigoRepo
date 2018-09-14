

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public  class EquipmentTypeDTO: IEntity
    {
        public int EquipmentTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool RowStatus { get; set; }
        public string ProductRequiredCode { get; set; }
        public string ChassisRequiredCode { get; set; }
        public string EquipmentNumberRequiredCode { get; set; }
    }
}
