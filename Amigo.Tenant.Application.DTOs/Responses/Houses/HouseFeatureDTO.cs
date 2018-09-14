using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseFeatureDTO
    {
        public int HouseFeatureId { get; set; }
        public int HouseId { get; set; }
        public int FeatureId { get; set; }
        public int HouseFeatureStatusId { get; set; }
        public bool IsRentable { get; set; }
        public bool RowStatus { get; set; }
        public string AdditionalAddressInfo { get; set; }
        public decimal RentPrice { get; set; }

        public string HouseFeatureStatusName { get; set; }

        public string FeatureCode { get; set; }
        public string FeatureDescription { get; set; }
        public string FeatureMeasure { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool Checked { get; set; }
        public int Sequence { get; set; }
    }
}
