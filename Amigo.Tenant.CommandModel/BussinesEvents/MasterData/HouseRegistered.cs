using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.BussinesEvents.MasterData
{
    public class HouseRegistered : IAsyncNotification
    {
        public int Id { get; set; }
    }
}
