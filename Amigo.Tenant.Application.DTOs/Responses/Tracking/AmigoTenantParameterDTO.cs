using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class AmigoTenantParameterDTO : IEntity
    {
        public int AmigoTenantParameterId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string IsForMobile { get; set; }
        public string IsForWeb { get; set; }

        public bool RowStatus { get; set; }
        

    }
}
