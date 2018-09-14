using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Product
{
    public class DeleteProductCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int ProductId { get; set; }
    }
}
