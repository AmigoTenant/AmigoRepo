using Amigo.Tenant.CrossCutting.Entities.Rules;
using System.Linq;

namespace Amigo.Tenant.CrossCutting.Entities.Dto
{
    public class OperationResult
    {
        public bool IsValid => BrokenRulesCollection.All(r => r.Severity != RuleSeverity.Error);
        public BrokenRulesCollection BrokenRulesCollection { get; set; } = new BrokenRulesCollection();
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(T data)
        {
            Data = data;
        }

        public OperationResult() {}

        public T Data { get; set; }
    }
}
