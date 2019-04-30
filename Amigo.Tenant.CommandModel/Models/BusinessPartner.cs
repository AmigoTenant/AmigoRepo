namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BusinessPartner")]
    public partial class BusinessPartner
    {
        public int BusinessPartnerId { get; set; }
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
        public int? BPTypeId { get; set; }

        [StringLength(12)]
        public string SIN { get; set; }

        public bool? RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual GeneralTable GeneralTable { get; set; }
    }
}
