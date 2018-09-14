using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
    public class ModuleSearchRequest: PagedRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool? OnlyParents { get; set; }
    }
}
