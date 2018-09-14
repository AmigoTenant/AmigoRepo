using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class MainTenantCodeNameRequest 
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string CodeAndFullName { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
