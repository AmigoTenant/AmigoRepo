using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Security
{
    public class Model : EntityBase
    {
        public Model()
        {
            Devices = new List<Device>();
        }

        public int ModelId { get; set; }
        public string Name { get; set; }
        public int? BrandId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}