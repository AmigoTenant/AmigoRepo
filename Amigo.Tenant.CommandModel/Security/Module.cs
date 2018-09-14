using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Amigo.Tenant.CommandModel.Abstract;
using models = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandModel.Security
{
    public class Module : EntityBase
    {
        public Module()
        {
            Actions = new List<models.Action>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string URL { get; set; }
        public int? ParentModuleId { get; set; }
        public int? SortOrder { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<models.Action> Actions { get; set; }
    }
}