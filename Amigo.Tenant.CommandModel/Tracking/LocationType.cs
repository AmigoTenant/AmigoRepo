using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Tracking
{
    public class LocationType : EntityBase
    {
        public LocationType()
        {
            Locations = new List<Location>();
        }

        public int LocationTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}