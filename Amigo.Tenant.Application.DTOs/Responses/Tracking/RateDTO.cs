namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public class RateDTO
    {
        public int? RateId      { get; set;}
        public string Code        { get; set;}
        public string Name        { get; set;}
        public string Description { get; set;}
        public string PaidBy      { get; set;}
        public int? ServiceId   { get; set;}
        public decimal? BillCustomer{ get; set;}
        public decimal? PayDriver   { get; set;}

    }
}
