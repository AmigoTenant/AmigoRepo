using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.CommandModel.Models
{
    public class AmigoTenantTUser : EntityBase
    {
        public AmigoTenantTUser()
        {
            Devices = new List<Device>();
            DriverReports = new List<DriverReport>();
            DriverReports1 = new List<DriverReport>();
            AmigoTenantTServices = new List<AmigoTenantTService>();
        }

        public int AmigoTenantTUserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AmigoTenantTRoleId { get; set; }
        public string PayBy { get; set; }
        public string UserType { get; set; }
        public int? DedicatedLocationId { get; set; }
        public bool? BypassDeviceValidation { get; set; }
        public string UnitNumber { get; set; }
        public string TractorNumber { get; set; }        
        public bool? RowStatus { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<DriverReport> DriverReports { get; set; }
        public virtual ICollection<DriverReport> DriverReports1 { get; set; }
        public virtual Location Location { get; set; }
        public virtual AmigoTenantTRole AmigoTenantTRole { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices { get; set; }
    }
}