using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Approve
{
    public class AmigoTenantTEventLogPerHourDTO
    {
        public int? AmigoTenantTEventLogId { get; set; }
        public string ActivityTypeCode { get; set; }
        public string ActivityTypeName { get; set; }
        public string Username { get; set; }
        //public int? DriverUserId { get; set; }
        public DateTime? ReportedActivityDateLocal { get; set; }
        public int? AmigoTenantTServiceId { get; set; }
        public int? AmigoTenantTUserId { get; set; }
    }
}
