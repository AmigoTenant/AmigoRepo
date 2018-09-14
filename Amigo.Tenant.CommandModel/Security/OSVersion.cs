using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Security
{
    public class OSVersion : EntityBase
    {
        public OSVersion()
        {
            Devices = new List<Device>();
        }

        public int OSVersionId { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public int? PlatformId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual Platform Platform { get; set; }
    }
}