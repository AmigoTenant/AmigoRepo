using Amigo.Tenant.CommandModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.Models
{
    public class ServicePeriod : EntityBase
    {
        public int ServicePeriodId { get; set; }
        public int? HouseServicePeriodId { get; set; }
        public int PeriodId { get; set; }
        public int? CompanyId { get; set; }
        public decimal Amount { get; set; }
        public decimal Adjust { get; set; }
        public int UM { get; set; }
        public decimal Comsumption { get; set; }
        public int ServicePeriodStatusId { get; set; }
        public int ServiceStatusId { get; set; }
        public bool RowStatus { get; set; }

        public HouseServicePeriod HouseServicePeriod { get; set; }
    }
}
