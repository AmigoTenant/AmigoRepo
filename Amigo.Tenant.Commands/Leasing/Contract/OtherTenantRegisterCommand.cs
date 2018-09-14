using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Commands.Leasing.Contracts
{
    public class OtherTenantRegisterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {

        public int? OtherTenantId { get; set; }

        public int? ContractId { get; set; }

        public int? TenantId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public ObjectStatus TableStatus { get; set; }

    }
}
