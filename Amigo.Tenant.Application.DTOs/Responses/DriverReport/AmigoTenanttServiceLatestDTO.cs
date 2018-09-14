using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Responses.Move
{
    public class AmigoTenanttServiceLatestDTO 
    {
        public int? AmigoTenantTUserId { get; set; }
        public DateTimeOffset? ServiceStartDate { get; set; }
        public DateTimeOffset? ServiceFinishDate { get; set; }
        public int? OriginLocationId { get; set; }
        public int? DestinationLocationId { get; set; }
        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }
        public string OriginLocationName { get; set; }
        public string DestinationLocationName { get; set; }
        public string DispatchingPartyCode { get; set; }
        public string ChargeNo { get; set; }
        public string ChargeType { get; set; }
    }
}
