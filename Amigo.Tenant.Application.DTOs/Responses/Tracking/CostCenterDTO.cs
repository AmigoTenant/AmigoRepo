
namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class CostCenterDTO: IEntity
    {
        public int CostCenterId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool RowStatus { get; set; }

    }
}
