using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Location
{

    public class DeleteLocationCoordinatesCommand : IAsyncRequest<CommandResult>
    {
        public string Code { get; set; }
    }
}
