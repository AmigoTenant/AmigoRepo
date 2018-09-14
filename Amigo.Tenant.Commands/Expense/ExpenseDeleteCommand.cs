using Amigo.Tenant.Commands.Common;
using MediatR;

namespace Amigo.Tenant.Commands.Expense
{
    public class ExpenseDeleteCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ExpenseId { get; set; }

    }
}
