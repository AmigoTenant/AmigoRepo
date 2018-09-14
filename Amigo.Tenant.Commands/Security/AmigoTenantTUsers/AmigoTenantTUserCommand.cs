using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTUsers
{
    public class AmigoTenantTUserCommand 
    {
        public string AmigoTenantTUserId { get; set; }
        public string Username { get; set; }
        public string PayBy { get; set; }
        public string UserType { get; set; }
        public int? DedicatedLocationId { get; set; }
        public bool? BypassDeviceValidation { get; set; }
        public string UnitNumber { get; set; }
        public string TractorNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleCode { get; set; }
        public bool RowStatus { get; set; }
        public int UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? AmigoTenantTRoleId { get; set; }

    }
}
