using System;
using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.CommandModel.Abstract
{

    public abstract class ValidatableBase :  IValidatable
    {
        private readonly List<string> _errors;

        protected ValidatableBase()
        {
            _errors = new List<string>();
        }


        #region Validatable

        public IEnumerable<string> Errors => _errors;
        public bool HasErrors => Errors.Any();


        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public void ClearErrors()
        {
            _errors.Clear();
        }

        #endregion
    }

}
