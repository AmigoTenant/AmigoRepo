using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class LocationRegistered : IAsyncNotification
    {
        public int LocationId { get; set; }
    }
}
