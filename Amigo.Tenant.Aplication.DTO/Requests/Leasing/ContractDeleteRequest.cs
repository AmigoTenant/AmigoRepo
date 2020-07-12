using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractDeleteRequest : AuditBaseRequest
    {
        public int? ContractId { get; set; }

    }
}
