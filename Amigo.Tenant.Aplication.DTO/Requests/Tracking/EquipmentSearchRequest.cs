namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class EquipmentSearchRequest 
    {
        public string EquipmentNo { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
