using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class OSVersionDTO
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public int OSVersionId { get; set; }
        public string VersionNumber { get; set; }
        public string VersionName { get; set; }

    }
}
