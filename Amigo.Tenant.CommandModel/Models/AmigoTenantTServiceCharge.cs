using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class AmigoTenantTServiceCharge: EntityBase
    {
        public int AmigoTenantTServiceChargeId { get; set; }
        public decimal? DriverPay { get; set; }
        public decimal? CustomerBill { get; set; }
        public int? AmigoTenantTServiceId { get; set; }
        public int? RateId { get; set; }
        public int? DriverReportId { get; set; }
        public virtual DriverReport DriverReport { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual AmigoTenantTService AmigoTenantTService { get; set; }
        public bool? RowStatus { get; set; }
    }
}