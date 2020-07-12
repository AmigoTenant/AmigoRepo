using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Notification
{
    public class SendNotificationRequest
    {
        public List<string> DestinationNumbers { get; set; }
        public string TextMessage { get; set; }
    }
}
