using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.Leasing.Contracts
{
    public class ContractDetailRegisterCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {

        public int ContractDetailId { get; set; }

        public DateTime DueDate { get; set; }

        public int ItemNo { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public decimal Rent { get; set; }

        public int ContractId { get; set; }

        public int ContractDetailStatusId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? PeriodId { get; set; }

        public int? DelayDays { get; set; }

        public decimal? FinePerDay { get; set; }

        public decimal? FineAmount { get; set; }

        public decimal? TotalPayment { get; set; }

        public int? PayTypeId { get; set; }

        public string PaymentReferenceNo { get; set; }

        public virtual ICollection<ContractDetailObligationRegisterCommand> ContractDetailObligations { get; set; }


    }
}
