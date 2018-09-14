using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class RegisterHouseServicePeriodCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
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
        public String UserName { get; set; }
        public int? PeriodId { get; set; }

    }
}
