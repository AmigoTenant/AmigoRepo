using System;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class AmigoTenantTServiceSearchRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To{get; set; }
        //public string Driver
        //{
        //    get; set;
        //}
        public int DriverId
        {
            get; set;
        }
        public string ChargeNoType
        {
            get; set;
        }
        public string EquipmentTypeCode
        {
            get; set;
        }

        public string EquipmentSizeCode
        {
            get; set;
        }
        public int EquipmentTypeId
        {
            get; set;
        }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
