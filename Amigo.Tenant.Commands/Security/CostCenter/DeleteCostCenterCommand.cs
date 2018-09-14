using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.CostCenter
{
    public class DeleteCostCenterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int CostCenterId
        {
            get; set;
        }
    }
}
