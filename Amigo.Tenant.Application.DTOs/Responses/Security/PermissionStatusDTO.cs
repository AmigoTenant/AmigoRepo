using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class PermissionStatusDTO 
    {
        public int PermissionId { get; set; }
        public int RowStatus { get; set; }

    }
}
