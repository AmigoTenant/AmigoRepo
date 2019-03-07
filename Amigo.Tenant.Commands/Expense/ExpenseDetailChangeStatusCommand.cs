using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.PaymentPeriod;
using MediatR;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Expense
{
    public class ExpenseDetailChangeStatusCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ExpenseDetailStatusId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ExpenseId { get; set; }
        public List<ExpenseDetailUpdateCommand> ExpenseDetail { get; set; }
        
    }
}
