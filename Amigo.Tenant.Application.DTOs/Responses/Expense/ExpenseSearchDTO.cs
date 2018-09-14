using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Expense
{
    public class ExpenseSearchDTO : IEntity
    {
        public int? ExpenseId { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public int? HouseId { get; set; }
        public string HouseName { get; set; }
        public int? HouseTypeId { get; set; }
        public int? PeriodId { get; set; }
        public string ReferenceNo { get; set; }
        public int? ExpenseDetailStatusId { get; set; }
        public string ExpenseDetailStatusName { get; set; }
        public string Remark { get; set; }
        public int? ConceptId { get; set; }
        public string ConceptName { get; set; }
        public string HouseTypeName { get; set; }
        public string TenantFullName { get; set; }
        public int? TenantId { get; set; }
        public decimal? Tax { get; set; }
        public decimal? SubTotalAmount { get; set; }
        public decimal? TotalAmount { get; set; }

    }
}
