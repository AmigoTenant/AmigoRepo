using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTUserAuditDTO 
    {
        public int? CreatedBy { get; set; }
        public string CreatedByCode { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string UpdatedByCode { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
