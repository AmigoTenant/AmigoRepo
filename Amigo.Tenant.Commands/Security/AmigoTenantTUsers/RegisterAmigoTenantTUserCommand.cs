using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTUsers
{
    public class RegisterAmigoTenantTUserCommand : AmigoTenantTUserCommand, IAsyncRequest<CommandResult>
    {
    }
}
