using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Leasing
{
    public class ContractHouseDetailDTO : IEntity
    {
        public int? ContractHouseDetailId { get; set; }

        public int? ContractId { get; set; }
        public int? HouseFeatureId { get; set; }
        public string FeatureCode  { get; set; }
        public string FeatureDescription { get; set; }

    }
}
