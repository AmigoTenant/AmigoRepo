

using System;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public class EquipmentDTO
    {
        public int EquipmentId { get; set; }
        public string EquipmentNo { get; set; }
        public string TestDate25Year { get; set; }
        public string TestDate5Year { get; set; }
        public int EquipmentSizeId { get; set; }
        public int EquipmentStatusId { get; set; }
        public int LocationId { get; set; }
        public string IsMasterRecord { get; set; }
        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int EquipmentTypeId
        {
            get; set;
        }
        public string EquipmentTypeCode { get; set; }
        public string EquipmentTypeName { get; set; }
        public string EquipmentSizeCode { get; set; }
        public string EquipmentSizeName { get; set; }
    
    }
}
