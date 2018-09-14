using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseDeleteRequest : AuditBaseRequest
    {
        public int? ExpenseId { get; set; }

    }
}
