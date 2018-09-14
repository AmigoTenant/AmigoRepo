using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Leasing
{
    public class ContractSearchDTO: IEntity
    {
        public bool? IsSelected
        {
            get; set;
        }
        public string ContractCode { get; set; }
        public string PeriodCode { get; set; }
        public string TenantFullName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string  HouseName { get; set; }
        public DateTime? NextDueDate { get; set; }
        public int? NextDaysToCollect { get; set; }
        public int? UnpaidPeriods { get; set; }
        public int? HouseId { get; set; }
        public int? ContractStatusId { get; set; }
        public int? ContractId { get; set; }
        public int? PeriodId { get; set; }
        public bool? RowStatus { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Features { get; set; }
        public decimal? RentDeposit { get; set; }
        public decimal? RentPrice { get; set; }
        public string ContractStatusCode { get; set; }
    }
}
