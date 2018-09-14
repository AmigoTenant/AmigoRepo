using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.BussinesEvents.MasterData
{
    public class MainTenantDeleted : IAsyncNotification
    {
        public int TenantId { get; set; }
    }
}
