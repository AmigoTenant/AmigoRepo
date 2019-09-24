using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractChangeStatusRequest : AuditBaseRequest
    {
        public int? ContractId { get; set; }
        //public decimal? RentPrice { get; set; }
        //public int? ContractStatusId { get; set; }
        //public bool? RowStatus { get; set; }
        public virtual ICollection<ContractDetailRegisterRequest> ContractDetails { get; set; }
        public virtual ICollection<PaymentPeriodRegisterRequest> PaymentsPeriod { get; set; }

    }
}
