using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class ExternalReportDTO
    {
        public int AmigoTenantTUserId
        {
            get; set;
        }
        public string Username
        {
            get; set;
        }
        public string Drivername
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }  //computed
        public string EquipmentNumber
        {
            get; set;
        }
        public string EquipmentSizeCode
        {
            get; set;
        }
        public string EquipmentSize
        {
            get; set;
        }
        public string EquipmentTypeCode
        {
            get; set;
        }
        public string EquipmentType
        {
            get; set;
        }
        public string ServiceCode
        {
            get; set;
        }
        public string Service
        {
            get; set;
        }
        public string Product
        {
            get; set;
        }
        public string IsHazardous
        {
            get; set;
        }
        public string IsHazardousLabel
        {
            get; set;
        } //computed
        public string ChargeNo
        {
            get; set;
        }//computed
        public string ChargeType
        {
            get; set;
        }
        public string OriginBlockCode
        {
            get; set;
        }
        public string OriginBlock
        {
            get; set;
        }
        public string DestinationBlockCode
        {
            get; set;
        }
        public string DestinationBlock
        {
            get; set;
        }
        public string Approver
        {
            get; set;
        }
        public bool? ServiceStatus
        {
            get; set;
        }
        public string ApprovalStatus
        {
            get; set;
        }//computed
        public string DispatchingParty
        {
            get; set;
        }
        public DateTimeOffset? ServiceStartDate
        {
            get; set;
        }
        public DateTimeOffset? ServiceFinishDate
        {
            get; set;
        }
        public Decimal CustomerBill
        {
            get; set;
        }
        public string EquipmentStatusName
        {
            get; set;
        }
        public string DriverComments
        {
            get; set;
        }
        public DateTime? ServiceStartDateLocal
        {
            get; set;
        }
        public string ServiceStartDayName
        {
            get; set;
        }
        public string ServiceFinishDayName
        {
            get; set;
        }
        public double ServiceTotalHours
        {
            get; set;
        }
        public int? EquipmentStatusId
        {
            get; set;
        }
        public string ChassisNo
        {
            get; set;
        }
        public int? ProductId
        {
            get; set;
        }
    }
}
