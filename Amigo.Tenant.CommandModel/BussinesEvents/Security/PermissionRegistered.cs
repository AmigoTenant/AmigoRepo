using MediatR;

namespace Amigo.Tenant.CommandModel.BussinesEvents.Security
{
    public class PermissionRegistered : IAsyncNotification
    {
        public int PermissionId { get; set; }
    }

    public class PermissionDeleted : IAsyncNotification
    {
        public PermissionDeleted(int permissionId)
        {
            this.PermissionId = permissionId;
        }

        public int PermissionId { get; set; }
    }
}