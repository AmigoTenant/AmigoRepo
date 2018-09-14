using XPO.ShuttleTracking.Mobile.Helpers;

namespace XPO.ShuttleTracking.Mobile.Infrastructure
{
    public interface IPermissions
    {
        void HasPermissions(PermissionsBuild obj);
        bool Has(PermissionsBuild obj);
        void RequestPermissions(PermissionsBuild obj);
        void RequestPermissionsReason(PermissionsBuild obj);
    }
}
