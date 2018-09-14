using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Security.Module
{
    public class DeleteModuleCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public string Code { get; set; }
    }
}
