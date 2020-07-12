using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.UtilityBills
{
    public class UtilityHouseServiceDTO
    {
        public int HouseServiceId { get; set; }
        public int HouseId { get; set; }
        public int ServiceId { get; set; }

        public int ConceptId { get; set; }
        public string ConceptCode { get; set; }
        public string ConceptDescription { get; set; }
        public int ConceptTypeId { get; set; }

        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; }

        public int ServiceTypeId { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeValue { get; set; }

        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
