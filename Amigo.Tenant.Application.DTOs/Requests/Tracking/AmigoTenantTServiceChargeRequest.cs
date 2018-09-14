using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
   public class AmigoTenantTServiceChargeRequest : BaseStatusRequest
    {
        public int AmigoTenantTServiceChargeId { get; set; }
        public decimal? DriverPay { get; set; }
        public decimal? CustomerBill { get; set; }
        public int? AmigoTenantTServiceId { get; set; }
        public int? RateId { get; set; }
        public int? DriverReportId { get; set; }

    }
}
