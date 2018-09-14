using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{

    public class CostCenterRegistered : IAsyncNotification
    {
        public int CostCenterId { get; set; }
    }

}