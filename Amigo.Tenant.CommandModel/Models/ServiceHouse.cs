using Amigo.Tenant.CommandModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandModel.Models
{
    public partial class ServiceHouse : EntityBase
    {
        public ServiceHouse()
        {
            ServiceHousePeriods = new HashSet<ServiceHousePeriod>();
        }

        public int ServiceId { get; set; }

        public int ConceptId { get; set; }

        public int BusinessPartnerId { get; set; }

        public int ServiceTypeId { get; set; }

        public bool RowStatus { get; set; }

        public GeneralTable ServiceType { get; set; }

        public BusinessPartner BusinessPartner { get; set; }

        public Concept Concept { get; set; }

        public virtual ICollection<ServiceHousePeriod> ServiceHousePeriods { get; set; }
    }
}
