using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{

    public class LocationCoordinateRegistered : IAsyncNotification
    {
        public int LocationCoordinateId { get; set; }
    }

}
