using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseWithCoordinatesDTO : HouseDTO
    {
        public HouseWithCoordinatesDTO()
        {
        }
        public HouseWithCoordinatesDTO(HouseDTO house)
        {
        }

        private void SetAttributes(HouseDTO house)
        {
            this.Address = house.Address;
            this.Code = house.Code;
            this.CreatedBy = house.CreatedBy;
            this.CreationDate = house.CreationDate;
            this.HouseId = house.HouseId;
            this.HouseStatusId = house.HouseStatusId;
            this.HouseTypeId = house.HouseTypeId;
            this.HouseTypeName = house.HouseTypeName;
            this.Name = house.Name;
            this.PhoneNumber = house.PhoneNumber;
            this.RentPrice = house.RentPrice;
            this.RowStatus = house.RowStatus;
            this.StatusCode = house.StatusCode;
            this.StatusName = house.StatusName;
            this.UpdatedBy = house.UpdatedBy;
            this.UpdatedDate = house.UpdatedDate;
        }

        public List<LocationCoordinateDTO> Coordinates { get; set; }
    }
}
