using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class ModuleDeleted : IAsyncNotification
    {
        public int ModuleId { get; set; }
    }
}
