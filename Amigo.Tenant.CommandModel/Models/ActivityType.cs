using System;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Models
{
    public class ActivityType
    {
        public ActivityType()
        {
            AmigoTenantTEventLogs = new List<AmigoTenantTEventLog>();
        }

        public int ActivityTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<AmigoTenantTEventLog> AmigoTenantTEventLogs { get; set; }
    }
}