using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class RequestLogRegistered : IAsyncNotification
    {
        public int RequestLogId { get; set; }
    }
}
