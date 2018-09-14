using System;


namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PPSearchDTO : IEntity
    {
        public bool? IsSelected
        {
            get; set;
        }
        public string PeriodCode { get; set; }
        public string ContractCode { get; set; }
        public int? ContractId { get; set; }
        public string TenantFullName { get; set; }
        public string  HouseName { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public int? ServicesPending { get; set; }
        public int? LateFeesPending { get; set; }
        public int? FinesPending { get; set; }
        public int? DepositPending { get; set; }
        public int? PeriodId { get; set; }
        public int? TenantId { get; set; }
        public int? HouseId { get; set; }
        public string PaymentPeriodStatusCode { get; set; }
        public string PaymentPeriodStatusName { get; set; }
        public decimal? PaymentAmount { get; set; }

        public decimal? DepositAmountPending { get; set; }
        public decimal? FinesAmountPending { get; set; }
        public decimal? ServicesAmountPending { get; set; }
        public decimal? LateFeesAmountPending { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
