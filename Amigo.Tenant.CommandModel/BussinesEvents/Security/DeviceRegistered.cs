using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{

    public class DeviceRegistered : IAsyncNotification
    {
        public int DeviceId { get; set; }
    }

}