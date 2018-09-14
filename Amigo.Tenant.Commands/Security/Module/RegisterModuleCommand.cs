using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Security.Module
{

    public class RegisterModuleCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        public string ParentModuleCode { get; set; }
        public int? SortOrder { get; set; }
        public List<ActionCommand> Actions { get; set; }

    }
}
