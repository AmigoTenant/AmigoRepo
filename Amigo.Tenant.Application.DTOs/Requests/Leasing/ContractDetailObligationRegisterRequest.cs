using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractDetailObligationRegisterRequest: AuditBaseRequest
    {

        public int ContractDetailObligationId { get; set; }

        public int? ContractDetailId { get; set; }

        public DateTime? ObligationDate { get; set; }

        public int? ConceptId { get; set; }

        public string Comment { get; set; }

        public decimal? InfractionAmount { get; set; }

        public int? TenantId { get; set; }

        public int? PeriodId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? TenantInfractorId { get; set; }

        public int? EntityStatusId { get; set; }


    }
}
