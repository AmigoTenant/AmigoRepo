using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class ModuleUpdated : IAsyncNotification
    {
        public int ModuleId { get; set; }
    }
}
