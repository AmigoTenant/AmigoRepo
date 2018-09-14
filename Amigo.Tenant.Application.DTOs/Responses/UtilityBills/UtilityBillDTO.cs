using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.UtilityBills
{
    public class UtilityBillDTO
    {
        public int HouseServicePeriodId { get; set; }
        public int HouseServiceId { get; set; }
        public int MonthId { get; set; }
        public int DueDateMonth { get; set; }
        public int DueDateDay { get; set; }
        public int CutOffMonth { get; set; }
        public int CutOffDay { get; set; }
        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? PeriodId { get; set; }

        public UtilityPeriodDTO Period { get; set; }
        public UtilityHouseServiceDTO HouseService { get; set; }
        public UtilityServicePeriodDTO ServicePeriod { get; set; }
    }
}
