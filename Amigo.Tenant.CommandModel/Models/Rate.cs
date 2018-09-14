using System;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Rate
    {
        public Rate()
        {
            AmigoTenantTServiceCharges = new List<AmigoTenantTServiceCharge>();
        }

        public int RateId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PaidBy { get; set; }
        public int? ServiceId { get; set; }
        public decimal? BillCustomer { get; set; }
        public decimal? PayDriver { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<AmigoTenantTServiceCharge> AmigoTenantTServiceCharges { get; set; }
        public virtual Service Service { get; set; }
    }
}