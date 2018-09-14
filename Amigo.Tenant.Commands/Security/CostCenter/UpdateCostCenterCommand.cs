using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.CostCenter
{
    public class UpdateCostCenterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int CostCenterId
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Code
        {
            get; set;
        }
    }
}
