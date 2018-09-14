using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTRole
{ 
    public class UpdateAmigoTenantTRoleCommand : AmigoTenantTRoleCommand, IAsyncRequest<CommandResult>
    {
    }
}
