using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.PaymentPeriod
{
    public class PaymentPeriodHeaderCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? PaymentPeriodId { get; set; }
        public int? PeriodId { get; set; }
        public string PeriodCode { get; set; }
        public string HouseName { get; set; }
        public string TenantFullName { get; set; }
        public List<PaymentPeriodDetailCommand> PPDetail { get; set; }
        ObjectStatus TableStatus { get; set; }
        public string Comment { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalRent { get; set; }
        public decimal? TotalDeposit { get; set; }
        public decimal? TotalLateFee { get; set; }
        public decimal? TotalService { get; set; }
        public decimal? TotalFine { get; set; }
        public decimal? TotalOnAcount { get; set; }
    }
}
