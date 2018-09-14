using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class HouseServicePeriodRequest : AuditBaseRequest
    {
        public int HouseServicePeriodId { get; set; }
        public int HouseServiceId { get; set; }
        public int MonthId { get; set; }

        public int DueDateMonth { get; set; }
        public int DueDateDay { get; set; }
        public int CutOffMonth { get; set; }
        public int CutOffDay { get; set; }

        public bool RowStatus { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreationDate { get; set; }

        public int? PeriodId { get; set; }

    }
}