using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTRoleBasicDTO 
    {
        public int AmigoTenantTRoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public bool IsAdmin { get; set; }

    }
}
