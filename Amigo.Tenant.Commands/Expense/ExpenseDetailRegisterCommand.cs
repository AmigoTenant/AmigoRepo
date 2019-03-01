using Amigo.Tenant.Commands.Common;
using MediatR;
using System;

namespace Amigo.Tenant.Commands.ExpenseDetail
{
    public class ExpenseDetailRegisterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? ExpenseDetailId { get; set; }
        public int? ExpenseId { get; set; }
        public int? ConceptId { get; set; }
        public string Remark { get; set; }
        public decimal? SubTotalAmount { get; set; }
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
        public int? ApplyTo { get; set; }
        //Campos para ingreso de varios detalles en funcion al apply To: Periodo y House en tabla PaymentPeriod
        public int PeriodId { get; set; }
        public int HouseId { get; set; }

    }
}
