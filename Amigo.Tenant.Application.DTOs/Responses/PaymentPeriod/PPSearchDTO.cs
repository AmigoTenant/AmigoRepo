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
        public int? RentPending { get; set; }
        public int? LateFeesPending { get; set; }
        public int? FinesPending { get; set; }
        public int? DepositPending { get; set; }
        public int? PeriodId { get; set; }
        public int? TenantId { get; set; }
        public int? HouseId { get; set; }
        public string PaymentPeriodStatusCode { get; set; }
        public string PaymentPeriodStatusName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? RentAmountPending { get; set; }
        public decimal? DepositAmountPending { get; set; }
        public decimal? FinesAmountPending { get; set; }
        public decimal? OnAccountAmountPending { get; set; }
        public decimal? LateFeesAmountPending { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? TotalExpenseAmount { get; set; }
        public decimal? TotalIncomePaidAmount { get; set; }
        public decimal? TotalIncomePendingAmount { get; set; }
        public decimal? TotalIncomeAmountByPeriod { get; set; }
        //PAID
        public decimal? RentAmountPaid { get; set; }
        public decimal? DepositAmountPaid { get; set; }
        public decimal? FinesAmountPaid { get; set; }
        public decimal? OnAccountAmountPaid { get; set; }
        public decimal? LateFeesAmountPaid { get; set; }
    }
}
