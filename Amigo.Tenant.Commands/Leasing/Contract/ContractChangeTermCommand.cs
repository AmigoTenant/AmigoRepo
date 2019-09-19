using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.PaymentPeriod;
using MediatR;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Leasing.Contracts
{
    public class ContractChangeTermCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ContractId { get; set; }
        public int? PeriodId { get; set; }
        public int? HouseId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? TenantId { get; set; }
        public decimal? newRent { get; set; }
        public decimal? newDeposit { get; set; }
    }
}
