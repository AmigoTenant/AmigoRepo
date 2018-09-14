using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class HouseServiceRequest : AuditBaseRequest
    {
        public int HouseServiceId { get; set; }
        public int HouseId { get; set; }
        public int ServiceId { get; set; }

        public bool RowStatus { get; set; }

        //public int CreatedBy { get; set; }
        //public DateTime CreationDate { get; set; }

        public IEnumerable<HouseServicePeriodRequest> HouseServicePeriods { get; set; }
    }

    public class HouseServiceMonthDayRequest
    {
        public string Month { get; set; }
        public string Day { get; set; }
    }
}
