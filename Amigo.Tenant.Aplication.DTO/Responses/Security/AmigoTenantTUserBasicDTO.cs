using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTUserBasicDTO : BaseDTO
    {
        public int AmigoTenantTUserId { get; set; }
        public string Username { get; set; }
        public string PayBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomUsername { get; set; }

    }
}
