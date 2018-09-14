using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.Permission
{
    public class DeletePermissionCommand : IAsyncRequest<CommandResult>
    {        
        public string CodeRol { get; set; }
        public string CodeAction { get; set; }
    }
}
