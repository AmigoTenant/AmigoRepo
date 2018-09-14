using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class MainTenantBasicDTO : IEntity
    {
        public int TenantId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public int TypeId { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int? ContractId { get; set; }
        public string ContractStatusCode { get; set; }

    }
}
