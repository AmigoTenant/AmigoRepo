using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Amigo.Tenant.Web.Logging
{
    public class RequestInfo
    {
        public string Url { get; private set; }
        public string ServiceName { get; private set; }
        public string Request { get; private set; }
        public string Response { get; private set; }
        public DateTime RequestDate { get; private set; }
        public int RequestBy { get; private set; }
        public RequestInfo(string url, string serviceName, string request, string response, DateTime requestDate, int requestBy)
        {
            Url = url;
            ServiceName = serviceName;
            Request = request;
            Response = response;
            RequestDate = requestDate;
            RequestBy = requestBy;
        }
    }
}
