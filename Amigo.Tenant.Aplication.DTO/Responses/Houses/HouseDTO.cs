using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseDTO : IEntity
    {
        public int HouseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public decimal RentPrice { get; set; }
        public bool RowStatus { get; set; }
        public int HouseTypeId { get; set; }
        public int HouseStatusId { get; set; }
        public string HouseTypeName { get; set; }
        public string PhoneNumber { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string ShortName { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string HouseTypeCode { get; set; }
        public int? CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
    }
}
