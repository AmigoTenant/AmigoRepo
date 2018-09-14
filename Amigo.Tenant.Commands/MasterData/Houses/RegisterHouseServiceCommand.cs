using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class RegisterHouseServiceCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public RegisterHouseServiceCommand()
        {
            HouseServicePeriods = new List<RegisterHouseServicePeriodCommand>();
        }

        public int HouseServiceId { get; set; }
        public int HouseId { get; set; }

        public int ServiceId { get; set; }

        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public String UserName { get; set; }

        public IEnumerable<RegisterHouseServicePeriodCommand> HouseServicePeriods { get; set; }
    }

    public class RegisterHouseServiceMonthDayCommand
    {
        public string Month { get; set; }
        public string Day { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Month, Day);
        }
    }
}
