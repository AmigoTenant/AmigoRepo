using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
   public class UpdateAmigoTenantTServiceAckRequest : MobileRequestBase
    {
        public int? AmigoTenantTServiceId { get; set; }
        public string AcknowledgeBy { get; set; }
        public DateTimeOffset? ServiceAcknowledgeDate { get; set; }
        public string ServiceAcknowledgeDateTZ { get; set; }
        //public DateTime? ServiceAcknowledgeDateUTC { get; set; }
        //public bool? IsAknowledged { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ServiceOrderNoRequest> ServiceOrderNoList { get; set; }
        public bool? IncludeRequestLog { get; set; }
    }
}
