using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
   public class CancelAmigoTenantServiceRequest : MobileRequestBase
    {
        public  int AmigoTenantTServiceId { get; set; }
        public bool? IncludeRequestLog { get; set; }
        public int? AmigoTenantTUserId { get; set; }

    }
}
