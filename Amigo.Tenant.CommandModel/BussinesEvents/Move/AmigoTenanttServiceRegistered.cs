

using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Move
{
    public    class AmigoTenanttServiceRegistered : IAsyncNotification
    {

        public int AmigoTenantTServiceId { get; set; }

        public AmigoTenantTEventLogDTO Log { get; set; }

    }
}
