using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseFeatureAndDetailDTO : IEntity
    {
        public int HouseFeatureId { get; set; }
        public string Description { get; set; }
        public bool? Marked { get; set; }
        public int? HouseId { get; set; }
        public bool? IsAllHouse { get; set; }
        public decimal? RentPrice { get; set; }
        public int? HouseFeatureStatusId { get; set; }
        public string HouseFetureStatusCode { get; set; }
        public int? TableStatus { get; set; }
        public bool? CouldBeDeleted { get; set; }
        public bool? IsDisabled { get; set; }
        public int? ContractId { get; set; }
        public int? Sequence { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ContractHouseDetailId { get; set; }
        public string AdditionalAddressInfo { get; set; }

    }
}
