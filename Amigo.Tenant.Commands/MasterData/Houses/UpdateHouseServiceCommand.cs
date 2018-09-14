using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class UpdateHouseServiceCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int HouseServiceId { get; set; }
        public int HouseId { get; set; }
        public string DueDateMonthDay { get; set; }
        public string InitialServiceMonthDay { get; set; }
        public string FinalServiceMonthDay { get; set; }
        public int ServiceId { get; set; }

        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public String UserName { get; set; }
    }
}
