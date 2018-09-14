using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class AmigoTenantTUserRegistered : IAsyncNotification
    {
        public int AmigoTenantTUserId { get; set; }
    }
}