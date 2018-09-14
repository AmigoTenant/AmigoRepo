using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Security
{
    public class AppVersion : EntityBase
    {
        public AppVersion()
        {
            Devices = new List<Device>();
        }

        public int AppVersionId { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseNotes { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}