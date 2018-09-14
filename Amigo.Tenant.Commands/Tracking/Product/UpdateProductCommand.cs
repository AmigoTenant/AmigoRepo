using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Product
{
    public class UpdateProductCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsHazardous { get; set; }
        public bool? IsHazardousBool { get; set; }

    }
}
