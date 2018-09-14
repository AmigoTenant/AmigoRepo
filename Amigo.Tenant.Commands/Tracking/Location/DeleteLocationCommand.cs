using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Location
{
    public class DeleteLocationCommand : IAsyncRequest<CommandResult>
    {
        public string Code { get; set; }
    }

}
