using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class Last24HoursRequest
    {
        public int? AmigoTenantTUserId
        {
            get; set;
        }
        public string TractorNumber
        {
            get; set;
        }
        public string ShipmentIdOrCostCenterCode
        {
            get; set;
        }
        public string ChassisNumber
        {
            get; set;
        }
        public string EquipmentNumber
        {
            get; set;
        }
        //public string ActivityTypeCode
        //{
        //    get; set;
        //}
        public List<string> ActivityTypeCodes
        {
            get; set;
        }
    }
}
