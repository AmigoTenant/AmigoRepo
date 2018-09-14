using Amigo.Tenant.Application.DTOs.Requests.Expense;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ExpenseDetailRegisterMapping : Map<ExpenseDetailRegisterRequest>
    {
        public ExpenseDetailRegisterMapping()
        {
            TableName("ExpenseDetail");
            //Columns(x =>
            //{
            //    //x.Column(y => y.Features).Ignore();
            //    //x.Column(y => y.Cities).Ignore();
            //});
            
        }
    }
}
