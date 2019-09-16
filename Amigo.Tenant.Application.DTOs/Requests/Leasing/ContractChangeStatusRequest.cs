using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class ContractChangeTermRequest : AuditBaseRequest
    {

        public int? ContractTermType { get; set; }
        public int? FinalPeriodId { get; set; }
        public int? FromPeriodId { get; set; }

        public int? NewTenantId { get; set; }

        public int? NewDeposit { get; set; }
        public int? NewRent { get; set; }

        public int ContractId { get; set; }
        public int TenantId { get; set; }
        public int HouseId { get; set; }


    }
}
