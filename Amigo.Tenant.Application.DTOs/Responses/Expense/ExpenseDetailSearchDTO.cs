using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Expense
{
    public class ExpenseDetailSearchDTO : IEntity
    {
        public int? ExpenseId { get; set; }
        public int? ExpenseDetailId { get; set; }
        public int? ExpenseDetailStatusId { get; set; }
        public string ExpenseDetailStatusName { get; set; }
        public int? ConceptId { get; set; }
        public string ConceptName { get; set; }
        public int? TenantId { get; set; }
        public string TenantFullName { get; set; }
        public string Remark { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }

    }
}
