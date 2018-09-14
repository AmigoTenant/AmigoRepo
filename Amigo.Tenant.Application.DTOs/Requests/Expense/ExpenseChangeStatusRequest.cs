using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseChangeStatusRequest 
    {
        public List<ChangeStatus> ChangeStatusList;
    }

    public class ChangeStatus
    {
        public int? ExpenseId { get; set; }
        public int? CurrentStatusId { get; set; }
        public int? NewStatusId { get; set; }

    }
}
