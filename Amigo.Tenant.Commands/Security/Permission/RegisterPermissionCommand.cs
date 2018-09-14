using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.Permission
{
    public class RegisterPermissionCommand : PermissionCommand, IAsyncRequest<CommandResult>
    {
      
    }
}
