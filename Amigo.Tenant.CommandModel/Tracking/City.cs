using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandModel.Tracking
{
    public class City : EntityBase
    {
        public City()
        {
            Locations = new List<Location>();
        }

        public int CityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? StateId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual State State { get; set; }
    }
}