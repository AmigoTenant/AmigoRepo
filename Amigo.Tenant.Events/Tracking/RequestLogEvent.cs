using System;
using MediatR;

namespace Amigo.Tenant.Events.Tracking
{
    public class RequestLogEvent : IAsyncNotification
    {
        public int RequestLogId { get; set; }
        public string URL { get; set; }
        public string ServiceName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
