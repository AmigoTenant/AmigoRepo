using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class DeleteHouseServiceRequest : AuditBaseRequest
    {
        public int HouseId { get; set; }
        public int HouseServiceId { get; set; }
    }
}
