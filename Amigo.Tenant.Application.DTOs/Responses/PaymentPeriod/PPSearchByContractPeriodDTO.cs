using System;


namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PPSearchByContractPeriodDTO : IEntity
    {
        public bool? IsSelected
        {
            get; set;
        }
        public int? PaymentPeriodId { get; set; }
        public int? PeriodId { get; set; }
        public string PeriodCode { get; set; }
        public string HouseName { get; set; }
        public string TenantFullName { get; set; }
        public string PaymentTypeValue { get; set; }
        public string PaymentDescription { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentPeriodStatusCode { get; set; }
        public string PaymentPeriodStatusName { get; set; }
        public int? ConceptId { get; set; }
        public int? ContractId { get; set; }
        public int? TenantId { get; set; }
        public string PaymentTypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Comment { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ReferenceNo { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsRequired { get; set; }
        public string PaymentTypeCode { get; set; }
        public DateTime? PeriodDueDate { get; set; }
        public int? PaymentTypeSequence { get; set; }
        public int? InvoiceDetailId { get; set; }
        public int? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Email { get; set; }
    }
}
