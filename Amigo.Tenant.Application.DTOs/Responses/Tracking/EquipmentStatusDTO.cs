
namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class EquipmentStatusDTO: IEntity
    {
    public int EquipmentStatusId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool RowStatus { get; set; }

}
}
