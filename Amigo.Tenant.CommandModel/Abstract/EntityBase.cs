using System;
using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.CommandModel.Abstract
{
    public abstract class EntityBase : ValidatableBase, IAuditable, IValidatable
    {
        private readonly List<string> _errors;

        #region Auditable

        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        #endregion

        public void Creation(int userId)
        {
            CreatedBy = userId;
            CreationDate = DateTime.UtcNow;
        }

        public void Update(int userId)
        {
            UpdatedBy = userId;
            UpdatedDate = DateTime.UtcNow;
        }


    }
}