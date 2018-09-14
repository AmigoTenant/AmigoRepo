using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class MoveRegistered : IAsyncNotification
    {
        public int MoveId { get; set; }
    }
}