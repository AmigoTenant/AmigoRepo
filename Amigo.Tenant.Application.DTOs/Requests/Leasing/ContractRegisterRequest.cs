using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractRegisterRequest: AuditBaseRequest
    {

        public int? ContractId { get; set; }
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int? MonthsNumber { get; set; }

        public decimal RentDeposit { get; set; }

        public decimal RentPrice { get; set; }

        public DateTime? ContractDate { get; set; }

        public int PaymentModeId { get; set; }

        public int ContractStatusId { get; set; }

        public int? PeriodId { get; set; }

        public string ContractCode { get; set; }

        public string ReferencedBy { get; set; }

        public int HouseId { get; set; }
        public string HouseName { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string FrecuencyTypeId { get; set; }

        public int? TenantId { get; set; }
        public string ContractStatusCode { get; set; }
        public string TenantCode { get; set; }
        public string FullName { get; set; }
        public string PeriodCode { get; set; }

        public virtual ICollection<ContractDetailRegisterRequest> ContractDetails { get; set; }
        public virtual ICollection<ContractHouseDetailRegisterRequest> ContractHouseDetails { get; set; }
        public virtual ICollection<OtherTenantRegisterRequest> OtherTenants { get; set; }
        public virtual ICollection<PaymentPeriodRegisterRequest> PaymentsPeriod { get; set; }

    }
}
