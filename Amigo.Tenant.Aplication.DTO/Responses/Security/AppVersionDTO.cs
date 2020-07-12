using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{

    public class AppVersionDTO
    {
        public int AppVersionId { get; set; }
        public string VersionNumber { get; set; }
        public string Name { get; set; }
    }
}
