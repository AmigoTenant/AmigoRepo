using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTUserDTO : BaseDTO
    {
        public int AmigoTenantTUserId { get; set; }
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
        public string Password { get; set; }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string AmigoTenantTRoleName { get; set; }
        public string LocationName { get; set; }
        public int? AmigoTenantTRoleId { get; set; }
        public string UserTypeName { get; set; }
        public string PayByName { get; set; }
        public string CustomUsername { get; set; }
        public int? DeviceId { get; set; }
        public string CellphoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAdminModifiedUser { get; set; }

    }
}
