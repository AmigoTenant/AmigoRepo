using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
  
    public class LocationCoordinateDeleted : IAsyncNotification
    {
        public int LocationId { get; set; }
    }
}
