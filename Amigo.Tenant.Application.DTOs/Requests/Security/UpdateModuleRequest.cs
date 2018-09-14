using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Security
{

    public class UpdateModuleRequest: AuditBaseRequest
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public string URL { get; set; }
        public string ParentModuleCode { get; set; }
        public int? SortOrder { get; set; }
        public List<ActionRequest> Actions { get; set; }


    }
}
