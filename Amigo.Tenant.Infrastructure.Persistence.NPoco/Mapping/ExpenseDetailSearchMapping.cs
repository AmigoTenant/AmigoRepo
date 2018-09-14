using Amigo.Tenant.Application.DTOs.Responses.Expense;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ExpenseDetailSearchMapping : Map<ExpenseDetailSearchDTO>
    {
        public ExpenseDetailSearchMapping()
        {
            TableName("vwExpenseDetailSearch");
            PrimaryKey(x => x.ExpenseDetailId);
            
        }
    }
}
