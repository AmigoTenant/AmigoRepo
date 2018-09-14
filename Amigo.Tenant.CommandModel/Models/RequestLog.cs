using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class RequestLog 
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
