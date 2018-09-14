using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.MainTenants
{
    public class DeleteMainTenantCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int TenantId { get; set; }
    }
}
