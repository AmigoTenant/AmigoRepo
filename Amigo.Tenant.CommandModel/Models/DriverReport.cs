using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class DriverReport: EntityBase
    {
        public DriverReport()
        {
            AmigoTenantTServiceCharges = new List<AmigoTenantTServiceCharge>();
        }

        public DriverReport(int createdBy)
        {
            AmigoTenantTServiceCharges = new List<AmigoTenantTServiceCharge>();
            if (createdBy>0)
                base.CreatedBy = createdBy;
        }

        public int DriverReportId { get; set; }
        public DateTime? ReportDate { get; set; }
        public int? Year { get; set; }
        public int? WeekNumber { get; set; }
        public int? DriverUserId { get; set; }
        public int? ApproverUserId { get; set; }
        public string ApproverSignature { get; set; }
        public decimal? DayPayDriverTotal { get; set; }
        public decimal? DayBillCustomerTotal { get; set; }
        public decimal? TotalHours { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public bool? RowStatus { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<AmigoTenantTServiceCharge> AmigoTenantTServiceCharges { get; set; }
        public virtual AmigoTenantTUser AmigoTenantTUser { get; set; }
        public virtual AmigoTenantTUser AmigoTenantTUser1 { get; set; }


    }
}