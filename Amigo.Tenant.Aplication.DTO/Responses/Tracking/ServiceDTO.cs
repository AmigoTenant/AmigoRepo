
namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class ServiceDTO : IEntity
    {
        public int ServiceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsPerMove { get; set; }
        public string IsPerHour { get; set; }
        public int ServiceTypeId { get; set; }
        public bool RowStatus { get; set; }
        public string ServiceTypeCode { get; set; }
        public string BlockRequiredCode { get; set; }
        public string ProductRequiredCode { get; set; }
        public string EquipmentRequiredCode { get; set; }
        public string ChassisRequiredCode { get; set; }
        public string DispatchingPartyRequiredCode { get; set; }
        public string EquipmentStatusRequiredCode { get; set; }
        public string BobtailAuthRequiredCode { get; set; }
        public string HasH34RequiredCode { get; set; }
    }                                                    
}
