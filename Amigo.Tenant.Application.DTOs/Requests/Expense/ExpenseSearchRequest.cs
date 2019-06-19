using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseSearchRequest : PagedRequest
    {
        public DateTime? ExpenseDateFrom { get; set; }
        public DateTime? ExpenseDateTo { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? HouseTypeId { get; set; }
        public int? PeriodId { get; set; }
        public string ReferenceNo { get; set; }
        public string Remark { get; set; }
        public decimal? TotalAmountFrom { get; set; }
        public decimal? TotalAmountTo { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string FileName { get; set; }
        public string PropertyName { get; set; }
        public string ConceptName { get; set; }


    }
}
