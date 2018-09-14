using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Leasing.Contracts
{
    public class ContractDeleteCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ContractId { get; set; }

    }
}
