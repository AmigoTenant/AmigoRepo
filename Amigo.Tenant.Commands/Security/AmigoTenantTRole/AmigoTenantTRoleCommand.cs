using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Permission;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTRole
{
    public class AmigoTenantTRoleCommand: BaseStatusRequest
    {
        public int AmigoTenantTRoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool RowStatus { get; set; }
        public List<PermissionCommand> Permissions { get; set; }

    }
}
