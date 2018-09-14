
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class RolPermissionDTO
    {

        public int ModuleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Actions> Items { get; set; }

    }
    public class Actions
    {
        public int ParentModuleId { get; set; }
        public int PermissionId { get; set; }
        public int ActionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool HasAction { get; set; }

    }

}
