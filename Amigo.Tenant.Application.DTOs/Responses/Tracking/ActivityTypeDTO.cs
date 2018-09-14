namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public  class ActivityTypeDTO: IEntity
    {
        public int ActivityTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool RowStatus { get; set; }
    }
}
