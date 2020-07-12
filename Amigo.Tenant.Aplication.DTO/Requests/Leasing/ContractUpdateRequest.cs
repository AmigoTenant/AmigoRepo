using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractUpdateRequest : AuditBaseRequest
    {
        public int? ContractId { get; set; }
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal RentDeposit { get; set; }

        public decimal RentPrice { get; set; }

        public DateTime? ContractDate { get; set; }

        public int PaymentModeId { get; set; }

        public int ContractStatusId { get; set; }

        public int? PeriodId { get; set; }

        public string ContractCode { get; set; }

        public string ReferencedBy { get; set; }

        public int HouseId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string FrecuencyTypeId { get; set; }

        public int? TenantId { get; set; }
        public virtual ICollection<ContractDetailRegisterRequest> ContractDetails { get; set; }
        public virtual ICollection<ContractHouseDetailRegisterRequest> ContractHouseDetails { get; set; }
        public virtual ICollection<OtherTenantRegisterRequest> OtherTenants { get; set; }

    }
}
