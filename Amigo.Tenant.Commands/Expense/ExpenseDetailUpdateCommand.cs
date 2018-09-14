using Amigo.Tenant.Commands.Common;
using MediatR;
using System;

namespace Amigo.Tenant.Commands.Expense
{
    public class ExpenseDetailUpdateCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ExpenseDetailId { get; set; }
        public int? ExpenseId { get; set; }
        public int? ConceptId { get; set; }
        public string Remark { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? TenantId { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ExpenseDetailStatusId { get; set; }
        public decimal? Quantity { get; set; }

    }
}
