using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class ProductDeleted : IAsyncNotification
    {
        public int ProductId { get; set; }

    }

}
