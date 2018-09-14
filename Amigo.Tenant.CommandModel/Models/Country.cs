using System;
using System.Collections.Generic;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Country
    {
        public Country()
        {
            States = new List<State>();
        }

        public int CountryId { get; set; }
        public string ISOCode { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<State> States { get; set; }
    }
}