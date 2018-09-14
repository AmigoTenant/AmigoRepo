using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTRoleDTO : BaseStatusRequest
    {
        public int? AmigoTenantTRoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsAdmin { get; set; }
        public bool RowStatus { get; set; }
        //public List<PermissionDTO> Permissions { get; set; } //modificar
    }
}
