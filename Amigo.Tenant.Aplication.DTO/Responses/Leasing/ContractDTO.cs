using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Leasing
{
    public class ContractDTO: IEntity
    {
        public int? ContractId { get; set; }
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal RentDeposit { get; set; }

        public decimal RentPrice { get; set; }

        public DateTime? ContractDate { get; set; }

        public int PaymentModeId { get; set; }

        public int ContractStatusId { get; set; }

        public string PeriodId { get; set; }

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
        public string ContractStatusCode { get; set; }
        public string TenantCode { get; set; }
        public string TenantFullName { get; set; }
        public string PeriodCode { get; set; }

        public List<OtherTenantDTO> OtherTenantsDTO { get; set; }

    }
}
