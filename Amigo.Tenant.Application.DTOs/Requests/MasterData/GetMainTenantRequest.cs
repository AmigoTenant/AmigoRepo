using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class GetMainTenantRequest
    {
        public int? Id { get; set; }
        public int? TypeId { get; set; }

    }
}
