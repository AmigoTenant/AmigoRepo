using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Tracking.Product
{
    public class RegisterProductCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsHazardous { get; set; }
        public bool? IsHazardousBool { get; set; }


    }
}
