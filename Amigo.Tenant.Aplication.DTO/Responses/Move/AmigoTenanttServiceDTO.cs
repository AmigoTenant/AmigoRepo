using System;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.DTOs.Responses.Move
{
    public class AmigoTenanttServiceDTO : MobileRequestBase /*, BaseDTO*/
    {
        public int AmigoTenantTServiceId { get; set; }
        public Guid ServiceOrderNo { get; set; }
        public DateTimeOffset? ServiceStartDate { get; set; }
        public string ServiceStartDateTZ { get; set; }
        public DateTimeOffset? ServiceFinishDate { get; set; }
        public string ServiceFinishDateTZ { get; set; }
        public DateTime? ServiceStartDateUTC { get; set; }
        public DateTime? ServiceFinishDateUTC { get; set; }
        public string EquipmentNumber { get; set; }
        public DateTime? EquipmentTestDate25Year { get; set; }
        public DateTime? EquipmentTestDate5Year { get; set; }
        public string ChassisNumber { get; set; }
        public string ChargeType { get; set; }
        public string ShipmentID { get; set; }
        public string AuthorizationCode { get; set; }
        public bool? HasH34 { get; set; }
        public int? DetentionInMinutesReal { get; set; }
        public int? DetentionInMinutesRounded { get; set; }
        public string AcknowledgeBy { get; set; }
        public DateTimeOffset? ServiceAcknowledgeDate { get; set; }
        public string ServiceAcknowledgeDateTZ { get; set; }
        public DateTime? ServiceAcknowledgeDateUTC { get; set; }
        public bool? IsAknowledged { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool? ServiceStatus { get; set; }
        public string ApprovalModified { get; set; }
        public int? OriginLocationId { get; set; }
        public int? DestinationLocationId { get; set; }
        public int? DispatchingPartyId { get; set; }
        public int? EquipmentSizeId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public int? EquipmentStatusId { get; set; }
        public int? CostCenterId { get; set; }
        public int? ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int? ServiceId { get; set; }
        public int? AmigoTenantTUserId { get; set; }
        public string PayBy { get; set; }
        public bool? IncludeRequestLog { get; set; }
        public string ChargeNo { get; set; }
        public string DriverComments { get; set; }

        public AmigoTenanttServiceDTO Clone()
        {
            var newDTO = new AmigoTenanttServiceDTO
            {
                IsAutoDateTime = this.IsAutoDateTime,
                IsSpoofingGPS = this.IsSpoofingGPS,
                IsRootedJailbreaked = this.IsRootedJailbreaked,
                Platform = this.Platform,
                OSVersion = this.OSVersion,
                AppVersion = this.AppVersion,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Accuracy = this.Accuracy,
                LocationProvider = this.LocationProvider,
                ReportedActivityTimeZone = this.ReportedActivityTimeZone,
                ReportedActivityDate = this.ReportedActivityDate,
                ActivityTypeId = (int) this.ActivityTypeId,
                PayBy = this.PayBy,

                AmigoTenantTServiceId = this.AmigoTenantTServiceId,
                ServiceOrderNo = this.ServiceOrderNo,
                ServiceStartDate = this.ServiceStartDate,
                ServiceStartDateTZ = this.ServiceStartDateTZ,
                ServiceFinishDate = this.ServiceFinishDate,
                ServiceFinishDateTZ = this.ServiceFinishDateTZ,
                ServiceStartDateUTC = this.ServiceStartDateUTC,
                ServiceFinishDateUTC = this.ServiceFinishDateUTC,
                EquipmentNumber = this.EquipmentNumber,
                EquipmentTestDate25Year = this.EquipmentTestDate25Year,
                EquipmentTestDate5Year = this.EquipmentTestDate5Year,
                ChassisNumber = this.ChassisNumber,
                ChargeType = this.ChargeType,
                ShipmentID = this.ShipmentID,
                AuthorizationCode = this.AuthorizationCode,
                HasH34 = this.HasH34,
                DetentionInMinutesReal = this.DetentionInMinutesReal,
                DetentionInMinutesRounded = this.DetentionInMinutesRounded,
                AcknowledgeBy = this.AcknowledgeBy,
                ServiceAcknowledgeDate = this.ServiceAcknowledgeDate,
                ServiceAcknowledgeDateTZ = this.ServiceAcknowledgeDateTZ,
                ServiceAcknowledgeDateUTC = this.ServiceAcknowledgeDateUTC,
                IsAknowledged = this.IsAknowledged,
                ApprovedBy = this.ApprovedBy,
                ApprovalDate = this.ApprovalDate,
                ServiceStatus = this.ServiceStatus,
                ApprovalModified = this.ApprovalModified,
                OriginLocationId = this.OriginLocationId,
                DestinationLocationId = this.DestinationLocationId,
                DispatchingPartyId = this.DispatchingPartyId,
                EquipmentSizeId = this.EquipmentSizeId,
                EquipmentTypeId = this.EquipmentTypeId,
                EquipmentStatusId = this.EquipmentStatusId,
                CostCenterId = this.CostCenterId,
                ProductId = this.ProductId,
                ProductDescription = this.ProductDescription,
                ServiceId = this.ServiceId,
                AmigoTenantTUserId = this.AmigoTenantTUserId,
                IncludeRequestLog = this.IncludeRequestLog,
                ChargeNo = this.ChargeNo,
                DriverComments = this.DriverComments
            };
            return newDTO;
        }
    }
}
