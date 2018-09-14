using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.CommandModel.Models
{
    public class AmigoTenantTService: EntityBase
    {
        public AmigoTenantTService()
        {
            AmigoTenantTServiceCharges = new List<AmigoTenantTServiceCharge>();
        }

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
        public int? ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int? ServiceId { get; set; }
        public int? AmigoTenantTUserId { get; set; }
        public bool? RowStatus { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public virtual DispatchingParty DispatchingParty { get; set; }
        public virtual EquipmentSize EquipmentSize { get; set; }
        public virtual EquipmentStatus EquipmentStatu { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual Product Product { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<AmigoTenantTServiceCharge> AmigoTenantTServiceCharges { get; set; }
        public virtual AmigoTenantTUser AmigoTenantTUser { get; set; }
        public string PayBy { get; set; }
        public string ApprovalComments { get; set; }
        
        public string ChargeNo { get; set; }
        public string DriverComments { get; set; }
    }
}