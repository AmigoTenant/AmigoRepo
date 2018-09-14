using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Expense
{
    public class ExpenseRegisterRequest : IEntity
    {
        public int? ExpenseId { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? HouseId { get; set; }
        public int? PeriodId { get; set; }
        public string ReferenceNo { get; set; }
        public string Remark { get; set; }
        public decimal? SubTotalAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }




    }
}
