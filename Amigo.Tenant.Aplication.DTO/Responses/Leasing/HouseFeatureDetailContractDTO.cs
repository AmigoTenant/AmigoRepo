using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Leasing
{
    public class HouseFeatureDetailContractDTO 
    {
        public int? ContractHouseDetailId { get; set; }
        public int? ContractId { get; set; }
        public int? HouseId { get; set; }
        public int? HouseFeatureId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
