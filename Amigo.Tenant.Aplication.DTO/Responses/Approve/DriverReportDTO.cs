using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Approve
{
    public class DriverReportDTO: BaseDTO
    {
        public DriverReportDTO(int? createdBy)
        {
            CreatedBy = createdBy > 0?createdBy: null;
            CreationDate = DateTime.UtcNow;
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
        //public bool? RowStatus { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public virtual List<AmigoTenantTServiceChargeDTO> AmigoTenantTServiceCharges { get; set; }
        
    }
}
