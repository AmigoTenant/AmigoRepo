using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Approve
{
    public class AmigoTenantTServiceChargeDTO//: BaseStatusRequest
    {
        //public AmigoTenantTServiceChargeDTO(int createdBy)
        //{
        //    CreatedBy = createdBy > 0 ? (int?)createdBy : null;
        //    CreationDate = DateTime.UtcNow;
        //}
        public int AmigoTenantTServiceChargeId { get; set; }
        public decimal? DriverPay { get; set; }
        public decimal? CustomerBill { get; set; }
        public int? AmigoTenantTServiceId { get; set; }
        public int? RateId { get; set; }
        public int? DriverReportId { get; set; }
    }
}
