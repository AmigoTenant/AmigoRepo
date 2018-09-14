using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Security;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Action : EntityBase
    {
        public Action()
        {
            Permissions = new List<Permission>();
        }

        public int ActionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? ModuleId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual Module Module { get; set; }
    }
}