using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Security
{
    public class Platform : EntityBase
    {
        public Platform()
        {
            OSVersions = new List<OSVersion>();
        }

        public int PlatformId { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<OSVersion> OSVersions { get; set; }
    }
}