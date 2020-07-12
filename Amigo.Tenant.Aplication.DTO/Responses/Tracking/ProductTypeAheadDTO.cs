using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class ProductTypeAheadDTO : IEntity
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
