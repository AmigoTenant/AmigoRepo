using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class PermissionDTO: BaseStatusRequest
    {
        public int PermissionId { get; set; }
        public int AmigoTenantTRoleId { get; set; }
        public int ActionId { get; set; }
        public string ActionCode { get; set; }
        public string AmigoTenantTRoleCode { get; set; }

    }
}
