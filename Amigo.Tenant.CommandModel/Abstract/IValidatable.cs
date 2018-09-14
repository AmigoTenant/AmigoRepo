using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Abstract
{
    public interface IValidatable
    {
        IEnumerable<string> Errors { get; }
        bool HasErrors { get; }
        void AddError(string error);
        void ClearErrors();
    }
}