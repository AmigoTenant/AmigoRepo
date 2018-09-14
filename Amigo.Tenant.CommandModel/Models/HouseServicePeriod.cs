using Amigo.Tenant.CommandModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.Models
{
    public partial class HouseServicePeriod : EntityBase
    {
        public HouseServicePeriod()
        {
            //ServicePeriods = new HashSet<ServicePeriod>();
        }

        public int HouseServicePeriodId { get; set; }

        public int HouseServiceId { get; set; }

        public int MonthId { get; set; }

        public int DueDateMonth { get; set; }

        public int DueDateDay { get; set; }

        public int CutOffMonth { get; set; }

        public int CutOffDay { get; set; }

        public bool RowStatus { get; set; }

        //public virtual ICollection<ServicePeriod> ServicePeriods { get; set; }

        public int? PeriodId { get; set; }
        public decimal Amount { get; set; }
        public decimal Adjust { get; set; }
        public decimal Consumption { get; set; }
        public int? ConsumptionUnmId { get; set; }
        public int? HouseServicePeriodStatusId { get; set; }

        public GeneralTable ConsumptionUnm { get; set; }

    }
}
