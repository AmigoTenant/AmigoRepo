using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Tracking;

namespace Amigo.Tenant.CommandModel.Models
{
    public class State
    {
        public State()
        {
            Cities = new List<City>();
        }

        public int StateId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual Country Country { get; set; }
    }
}