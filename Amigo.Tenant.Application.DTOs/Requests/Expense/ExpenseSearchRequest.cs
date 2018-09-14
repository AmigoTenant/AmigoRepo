using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseSearchRequest : PagedRequest
    {
        public int? ExpenseId { get; set; }
        public DateTime? ExpenseDateFrom { get; set; }
        public DateTime? ExpenseDateTo { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? HouseId { get; set; }
        public int? HouseTypeId { get; set; }
        public int? PeriodId { get; set; }
        public string ReferenceNo { get; set; }
        public int? ExpenseDetailStatusId { get; set; }
        public string Remark { get; set; }
        public int? ConceptId { get; set; }

    }
}
