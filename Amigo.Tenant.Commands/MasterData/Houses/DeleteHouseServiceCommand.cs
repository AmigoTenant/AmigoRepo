using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class DeleteHouseServiceCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int HouseId { get; set; }
        public int HouseServiceId { get; set; }
    }
}
