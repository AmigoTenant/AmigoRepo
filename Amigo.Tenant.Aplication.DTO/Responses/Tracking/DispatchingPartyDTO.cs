namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public class DispatchingPartyDTO: IEntity
    {
        public int DispatchingPartyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool RowStatus { get; set; }
            
    }
}
