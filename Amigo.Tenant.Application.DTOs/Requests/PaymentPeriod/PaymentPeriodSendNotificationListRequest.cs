namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodSendNotificationListRequest
    {
        public int ContractId { get; set; }
        public int PeriodId { get; set; }
        public int PeriodCode { get; set; }
    }
}
