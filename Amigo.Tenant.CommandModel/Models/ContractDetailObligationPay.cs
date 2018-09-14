namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("ContractDetailObligationPay")]
    public partial class ContractDetailObligationPay
    {
        [Key]
        public int ContractDetailInfractionPayId { get; set; }

        public int? ItemNo { get; set; }

        public int? ContractDetailObligationId { get; set; }

        public decimal? PayAmount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PayDate { get; set; }

        public int? TenantId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual ContractDetailObligation ContractDetailObligation { get; set; }

        public virtual MainTenant Tenant { get; set; }
    }
}
