using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Security.CostCenter
{

    public class RegisterCostCenterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public string Code
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }
}
