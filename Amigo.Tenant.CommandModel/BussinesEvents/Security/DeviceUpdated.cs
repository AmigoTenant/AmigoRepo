using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{

    public class DeviceUpdated : IAsyncNotification
    {
        public int DeviceId { get; set; }
    }
}
