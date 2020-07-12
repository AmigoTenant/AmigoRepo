using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class MainTenantSearchRequest
    {
        public int? TenantId  { get; set; }
        public int? StatusId  { get; set; }
        public int? CountryId { get; set; }
        public int? TypeId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool? RowStatus { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
