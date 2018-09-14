namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    //[Table("Period")]
    public partial class Period
    {
        public int PeriodId { get; set; }

        //[StringLength(6)]
        public string Code { get; set; }

        //[Column(TypeName = "datetime2")]
        public DateTime? BeginDate { get; set; }

        //[Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int Sequence { get; set; }

    }
}
