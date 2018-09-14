using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class LocationDeleted : IAsyncNotification
    {
        public int LocationId { get; set; }
    }
}
