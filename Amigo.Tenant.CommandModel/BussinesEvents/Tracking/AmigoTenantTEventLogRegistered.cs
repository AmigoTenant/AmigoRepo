using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Tracking
{
    public class AmigoTenantTEventLogRegistered : IAsyncNotification
    {
        public int AmigoTenantTEventLogId { get; set; }
    }
}
