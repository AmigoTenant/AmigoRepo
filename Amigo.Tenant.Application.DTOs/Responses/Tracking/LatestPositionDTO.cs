using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class LatestPositionDTO
    {
        public int AmigoTenantTEventLogId { get; set; }
        public int AmigoTenantTUserId { get; set; }
        public string Username { get; set;}
        public string ReportedActivityTimeZone { get; set; }
        public DateTimeOffset? ReportedActivityDate { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityTypeCode { get; set; }
        public string TractorNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ChargeNo { get; set; }

    }

}
