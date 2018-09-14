namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("IncomeDetail")]
    public partial class IncomeDetail
    {
        public int IncomeDetailId { get; set; }

        public int? IncomeId { get; set; }

        public int? ContractDetailId { get; set; }

        public int? ConceptId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? Qty { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? ItemNo { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? ContractDetailObligationId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual Concept Concept { get; set; }

        public virtual ContractDetail ContractDetail { get; set; }

        public virtual ContractDetailObligation ContractDetailObligation { get; set; }

        public virtual Income Income { get; set; }
    }
}
