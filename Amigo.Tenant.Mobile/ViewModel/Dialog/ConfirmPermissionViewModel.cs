using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using XPO.ShuttleTracking.Mobile.Helpers;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.ViewModel.Dialog
{
    public class ConfirmPermissionViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IPermissions _permissions;
        public  Action ForShow { get; set; }                
        public bool isAccepted { get; set; }
        public ConfirmPermissionViewModel(INavigator navigator,
            IPermissions permissions)
        {
            if (navigator == null) throw new ArgumentNullException(nameof(navigator));
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));
            _navigator = navigator;
            _permissions = permissions;
        }

        public ICommand AcceptCommand => CreateCommand(async () =>
        {
            if (await SetPermissions("", false))
            {
                isAccepted = true;
                await _navigator.PopModalAsync();
                ForShow();                
            }
        });

        public async Task<bool> SetPermissions(string informationMessage, bool onlyCheck)
        {
            var o = new PermissionsBuild
            {                
                PermissionToTest = string.Empty,
                Reason = informationMessage,
                AsPopup = true
            };
            if (onlyCheck)
            {
                return _permissions.Has(o);
            }
            _permissions.RequestPermissionsReason(o);
            return await o.Result.Task;
        }

    }
}
