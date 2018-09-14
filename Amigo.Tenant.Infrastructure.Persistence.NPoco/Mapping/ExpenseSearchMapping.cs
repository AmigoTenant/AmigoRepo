using Amigo.Tenant.Application.DTOs.Responses.Expense;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ExpenseSearchMapping : Map<ExpenseSearchDTO>
    {
        public ExpenseSearchMapping()
        {
            TableName("vwExpenseSearch");
            PrimaryKey(x => x.ExpenseId);
            
        }
    }
}
