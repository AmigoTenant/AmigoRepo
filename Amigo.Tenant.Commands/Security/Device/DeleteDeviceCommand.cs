using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.Device
{
    public class DeleteDeviceCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int DeviceId { get; set; }
    }
}
