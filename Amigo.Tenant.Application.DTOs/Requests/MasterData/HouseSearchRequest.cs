using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class HouseSearchRequest : PagedRequest
    {
        public string Address { get; set; }
        public int? HouseTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? RowStatus { get; set; }
        public int? HouseStatusId { get; set; }
    }
}
