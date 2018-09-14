using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class ModuleRegistered : IAsyncNotification
    {
        public int ModuleId { get; set; }
    }
}
