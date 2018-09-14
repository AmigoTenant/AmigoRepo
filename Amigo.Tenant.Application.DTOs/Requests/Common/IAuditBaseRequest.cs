using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Common
{
    public interface IAuditBaseRequest
    {
        int? UserId { get; set; }
    }
}
