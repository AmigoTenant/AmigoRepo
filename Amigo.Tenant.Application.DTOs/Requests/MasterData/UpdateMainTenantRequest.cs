using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class UpdateMainTenantRequest : AuditBaseRequest
    {
        public string TenantId { get; set; }
        public string StatusId { get; set; }
        public string CountryId { get; set; }
        public string TypeId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string PhoneN01 { get; set; }
        //public string UserId { get; set; }
        //public string Username { get; set; }
        public string IdRef { get; set; }

        public string PassportNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Reference { get; set; }
        public string PhoneNo2 { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactRelation { get; set; }
        public string ContactPhone { get; set; }

        public bool RowStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
