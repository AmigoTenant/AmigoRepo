using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class RegisterMoveRequest
    {
        public int CostCenterId { get; set; }
        public int ProductId { get; set; }        
    }
}
