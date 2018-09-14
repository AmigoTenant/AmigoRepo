using System;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentNo { get; set; }
        public int? EquipmentTypeId { get; set; }
        public DateTime? TestDate25Year { get; set; }
        public DateTime? TestDate5Year { get; set; }
        public int? EquipmentSizeId { get; set; }
        public int? EquipmentStatusId { get; set; }
        public int? LocationId { get; set; }
        public bool? IsMasterRecord { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual EquipmentSize EquipmentSize { get; set; }
        public virtual EquipmentStatus EquipmentStatu { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual Location Location { get; set; }
    }
}