using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
   public class UpdateAmigoTenantServiceRequest : MobileRequestBase
    {
        public  int AmigoTenantTServiceId { get; set; }
        public DateTimeOffset? ServiceFinishDate { get; set; }
        public string ServiceFinishDateTZ { get; set; }
        public DateTime? ServiceFinishDateUTC { get; set; }
        public int? DestinationLocationId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid ServiceOrderNo { get; set; }
        public int? AmigoTenantTUserId { get; set; }
        public bool? IncludeRequestLog { get; set; }
        public string ChargeType { get; set; }
        public string ShipmentID { get; set; }
        public int? CostCenterId { get; set; }
        public string DriverComments { get; set; }
        public int? EquipmentStatusId { get; set; }
        
        // public AmigoTenantTEventLogDTO Log { get; set; }

    }
}
