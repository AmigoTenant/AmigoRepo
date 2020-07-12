﻿using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod
{
    public class PaymentPeriodRegisterRequest: AuditBaseRequest
    {
        public int PaymentPeriodId { get; set; }
        public int? ConceptId              { get; set; }
        public int? ContractId             { get; set; }
        public int? TenantId               { get; set; }
        public int? PeriodId               { get; set; }
        public decimal? PaymentAmount          { get; set; }
        public DateTime? DueDate                { get; set; }
        public int? PaymentPeriodStatusId { get; set; }
        public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? PaymentTypeId { get; set; }
        public string Comment { get; set; }
        public string ReferenceNo { get; set; }
        public int? HouseId { get; set; }
    }
}