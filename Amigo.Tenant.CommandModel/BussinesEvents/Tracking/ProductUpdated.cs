using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{

    public class ProductUpdated : IAsyncNotification
    {
        public int ProductId { get; set; }
    }
}
