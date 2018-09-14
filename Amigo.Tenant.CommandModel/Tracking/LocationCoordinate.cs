using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandModel.Tracking
{
    public class LocationCoordinate : ValidatableBase
    {
        public int LocationCoordinateId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DbGeometry Coordinate { get; set; }
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

    }
}