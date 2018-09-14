using Amigo.Tenant.Commands.Common;
using MediatR;

namespace Amigo.Tenant.Commands.Expense
{
    public class ExpenseDetailDeleteCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ExpenseDetailId { get; set; }

    }
}
