using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Commands.Security.Permission
{
    public class PermissionCommand: BaseStatusRequest
    {
        public int PermissionId { get; set; }
        public int ActionId { get; set; }
        public string CodeRol { get; set; }
    }
}
