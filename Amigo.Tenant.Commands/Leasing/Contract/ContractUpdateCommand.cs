using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Leasing.Contracts
{
    public class ContractUpdateCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
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
        public virtual ICollection<ContractDetailRegisterCommand> ContractDetails { get; set; }
        public virtual ICollection<ContractHouseDetailRegisterCommand> ContractHouseDetails { get; set; }
        public virtual ICollection<OtherTenantRegisterCommand> OtherTenants { get; set; }

    }
}
