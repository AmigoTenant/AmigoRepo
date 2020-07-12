using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;


namespace Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod
{
    public class PPDetailSearchByInvoiceDTO : IEntity
    {
        public bool? IsSelected
        {
            get; set;
        }
        public int? PaymentPeriodId { get; set; }
        public int? ContractId { get; set; }
        public string PaymentTypeValue { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentDescription { get; set; }
        public Decimal? PaymentAmount { get; set; }
        public DateTime? PeriodDueDate { get; set; }
        public int? PaymentTypeSequence { get; set; }

    }
}
