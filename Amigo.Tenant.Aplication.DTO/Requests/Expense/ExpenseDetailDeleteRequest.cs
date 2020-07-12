using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseDetailDeleteRequest : AuditBaseRequest
    {
        public int? ExpenseDetailId { get; set; }

    }
}
