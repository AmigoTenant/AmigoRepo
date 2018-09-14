using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTRole
{
    public class DeleteAmigoTenantTRoleCommand : IAsyncRequest<CommandResult>
    {
        public string AmigoTenantTRoleId { get; set; }
        public string Code { get; set; }
        public bool RowStatus { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
