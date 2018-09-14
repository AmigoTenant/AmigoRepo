using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseDetailChangeStatusRequest 
    {
        public List<ChangeStatusDetail> ChangeStatusList;
    }

    public class ChangeStatusDetail
    {
        public int? ExpenseDetailId { get; set; }
        public int? CurrentStatusId { get; set; }
        public int? NewStatusId { get; set; }

    }
}
