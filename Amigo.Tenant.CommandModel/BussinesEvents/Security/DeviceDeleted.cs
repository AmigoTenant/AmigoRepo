using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class DeviceDeleted : IAsyncNotification
    {
        public int DeviceId { get; set; }
    }

}
