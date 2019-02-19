using Amigo.Tenant.Application.DTOs.Responses.Expense;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ExpenseDtoMapping : Map<ExpenseDTO>
    {
        public ExpenseDtoMapping()
        {
            TableName("Expense");
        }
    }
}
