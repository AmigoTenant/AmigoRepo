using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Commands.Security.Module
{
    public class ActionCommand : BaseStatusRequest
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }


    }
}
