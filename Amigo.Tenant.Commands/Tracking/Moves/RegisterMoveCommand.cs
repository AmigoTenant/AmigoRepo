using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Moves
{
    public class RegisterMoveCommand: IAsyncRequest<CommandResult>
    {
        public int DriverId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationId  { get; set; }
        public DateTimeOffset RequestedDate { get; set; }
    }
}
