namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("OtherTenant")]
    public partial class OtherTenant
    {
        [Key]
        public int OtherTenantId { get; set; }

        public int? ContractId { get; set; }

        public int? TenantId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public virtual Contract Contract { get; set; }

        public virtual MainTenant Tenant { get; set; }
        
    }
}
