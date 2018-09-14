using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class CostCenterDeleted : IAsyncNotification
    {
        public int CostCenterId
        {
            get; set;
        }
    }

}
