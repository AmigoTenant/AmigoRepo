using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Security
{
    public class Brand : EntityBase
    {
        public Brand()
        {
            Models = new List<Model>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Model> Models { get; set; }
    }
}