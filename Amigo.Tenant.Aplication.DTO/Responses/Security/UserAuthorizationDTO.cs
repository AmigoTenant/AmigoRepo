using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Response.Security
{
    public class UserAuthorizationDTO
    {
        public string AssignedAmigoTenantTUserUsername { get; set; }
        public int? DedicatedLocationId { get; set; }
        public bool? BypassDeviceValidation { get; set; }
        public string DedicatedLocationCode { get; set; }
        public DateTime? UserUpdatedDate { get; set; }
        public int? DeviceId { get; set; }
        public string CellphoneNumber { get; set; }
        public string Identifier { get; set; }
        public string PayBy { get; set; }
        public int? AmigoTenantTUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
