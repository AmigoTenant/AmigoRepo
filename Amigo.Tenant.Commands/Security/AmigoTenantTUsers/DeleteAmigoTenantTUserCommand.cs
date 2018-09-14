using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.AmigoTenantTUsers
{
    public class DeleteAmigoTenantTUserCommand : IAsyncRequest<CommandResult>
    {
        public string AmigoTenantTUserId { get; set; }
        public string Username { get; set; }
        public bool RowStatus { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
