using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandModel.Tracking
{
    public class Location:EntityBase
    {
        public Location()
        {
            Equipments = new List<Equipment>();
            LocationCoordinates = new List<LocationCoordinate>();
            AmigoTenantTServices = new List<AmigoTenantTService>();
            AmigoTenantTServices1 = new List<AmigoTenantTService>();
            AmigoTenantTUsers = new List<AmigoTenantTUser>();
        }

        public int LocationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? LocationTypeId { get; set; }
        public int? ParentLocationId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DbGeometry Coordinate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public int? CityId { get; set; }
        public bool? RowStatus { get; set; }
        public bool? HasGeofence { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<LocationCoordinate> LocationCoordinates { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices1 { get; set; }
        public virtual ICollection<AmigoTenantTUser> AmigoTenantTUsers { get; set; }
        public virtual LocationType LocationType { get; set; }
    }
}