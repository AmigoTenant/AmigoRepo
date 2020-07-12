namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class DispatchingPartySearchRequest 
    {
        public int DispatchingPartyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
