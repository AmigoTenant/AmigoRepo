using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseServicePeriodDTO
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
        public decimal? Amount { get; set; }
        public decimal? Adjust { get; set; }

        public decimal? Consumption { get; set; }
        public int? ConsumptionUnmId { get; set; }
        public int? HouseServicePeriodStatusId { get; set; }
    }
}
