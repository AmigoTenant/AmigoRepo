using Amigo.Tenant.CommandModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.Models
{
    public class ServiceHousePeriod : EntityBase
    {
        public int ServiceHousePeriodId { get; set; }
        public int MonthId { get; set; }
        public int ServiceId { get; set; }
        public int DueDateMonth { get; set; }
        public int DueDateDay { get; set; }
        public int CutOffMonth { get; set; }
        public int CutOffDay { get; set; }

        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
