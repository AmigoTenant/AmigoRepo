using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Move
{
    public class AmigoTenantTServiceReportDTO //: MobileRequestBase /*, BaseDTO*/
    {
        public bool? IsSelected
        {
            get; set;
        }
        public string AmigoTenantTServiceId
        {
            get; set;
        }
        public string CostCenterCode
        {
            get; set;
        }
        public string ChargeNoType
        {
            get; set;
        }
        public string ServiceCode
        {
            get; set;
        }
        public string ServiceName;
        public string EquipmentSizeCode
        {
            get; set;
        }
        public string EquipmentTypeCode
        {
            get; set;
        }
        public string EquipmentTypeName
        {
            get; set;
        }
        public int EquipmentTypeId
        {
            get; set;
        }
        public string EquipmentStatusCode
        {
            get; set;
        }
        public string EquipmentStatusName
        {
            get; set;
        }
        public int? EquipmentStatusId
        {
            get; set;
        }
        public string OriginLocationCode
        {
            get; set;
        }
        public string OriginLocationName
        {
            get; set;
        }
        public string DestinationLocationCode
        {
            get; set;
        }
        public int? OriginLocationId
        {
            get; set;
        }
        public int? DestinationLocationId
        {
            get; set;
        }
        public string DestinationLocationName
        {
            get; set;
        }
        public string ProductCode
        {
            get; set;
        }

        public string ProductName
        {
            get; set;
        }
        public string DispatchingPartyCode
        {
            get; set;
        }
        public string EquipmentNumber
        {
            get; set;
        }
        public string ChassisNumber
        {
            get; set;
        }
        public string ShipmentID
        {
            get; set;
        }
        public string ApprovedBy
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

        public int? AmigoTenantTUserId
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
        //public string Approve { get; set; }
        public string ChargeType
        {
            get; set;
        }

        public int? ServiceId
        {
            get; set;
        }
        public int? ProductId
        {
            get; set;
        }
        public bool? HasH34
        {
            get; set;
        }

        public bool? ServiceStatus
        {
            get; set;
        }

        //public string ChargeNoTypeDesc { get; set; }
        //public string PayByDesc { get; set; }
        public string AcknowledgeBy
        {
            get; set;
        }
        public string PayBy
        {
            get; set;
        }
        public int? EquipmentSizeId
        {
            get; set;
        }
        public int? DispatchingPartyId
        {
            get; set;
        }
        public DateTime? EquipmentTestDate25Year
        {
            get; set;
        }
        public DateTime? EquipmentTestDate5Year
        {
            get; set;
        }
        public string AuthorizationCode
        {
            get; set;
        }
        public string ActivityCode
        {
            get; set;
        }

        public string ServiceTypeCode
        {
            get; set;
        }
        public string ChargeNo
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
        public string ApprovalComments
        {
            get; set;
        }

        public int? CreatedBy
        {
            get; set;
        }
        public DateTime? CreationDate
        {
            get; set;
        }
        public int? UpdatedBy
        {
            get; set;
        }
        public DateTime? UpdatedDate
        {
            get; set;
        }

    }
}


