using System;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Models
{
    public class EquipmentSize
    {
        public EquipmentSize()
        {
            Equipments = new List<Equipment>();
            AmigoTenantTServices = new List<AmigoTenantTService>();
        }

        public int EquipmentSizeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices { get; set; }
    }
}