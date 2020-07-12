using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class AuthorizationRequest : MobileRequestBase
    {
        public string Identifier { get; set; }
        public bool? IncludeRequestLog { get; set; }
    }
}
