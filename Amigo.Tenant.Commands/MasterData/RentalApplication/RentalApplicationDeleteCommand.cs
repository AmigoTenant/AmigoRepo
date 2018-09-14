using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.MasterData.RentalApplication
{
    public class RentalApplicationDeleteCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? RentalApplicationId { get; set; }

    }
}
