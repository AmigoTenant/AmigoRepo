using System;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class FeatureDTO: IEntity
    {
        public int? FeatureId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Measure { get; set; }
        public bool RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int HouseTypeId { get; set; }
        public int Sequence { get; set; }
        public bool IsAllHouse { get; set; }

        public string HouseTypeCode { get; set; }

    }
}