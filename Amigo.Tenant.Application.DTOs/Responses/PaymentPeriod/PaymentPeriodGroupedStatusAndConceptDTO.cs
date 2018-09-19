namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PaymentPeriodGroupedStatusAndConceptDTO : IEntity
    {
        public int? PeriodId { get; set; }
        public int? TenantId { get; set; }
        public decimal? TotalOnAccountAmountPayed { get; set; }
        public decimal? TotalRentAmountPending { get; set; }
        public decimal? TotalDepositAmountPending { get; set; }
        public decimal? TotalSvcWaterAmountPending { get; set; }
        public decimal? TotalLateFeeAmountPending { get; set; }
        public decimal? TotalSvcEnergyAmountPending { get; set; }
        public decimal? TotalFineInfracAmountPending { get; set; }
    }
}
