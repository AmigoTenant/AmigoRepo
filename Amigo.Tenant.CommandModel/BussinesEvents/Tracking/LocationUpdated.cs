using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{

    public class LocationUpdated : IAsyncNotification
    {
        public int LocationId { get; set; }
    }
}
