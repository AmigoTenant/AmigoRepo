using System;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Service
    {
        public Service()
        {
            Rates = new List<Rate>();
            AmigoTenantTServices = new List<AmigoTenantTService>();
        }

        public int ServiceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsPerMove { get; set; }
        public string IsPerHour { get; set; }
        public int? ServiceTypeId { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}