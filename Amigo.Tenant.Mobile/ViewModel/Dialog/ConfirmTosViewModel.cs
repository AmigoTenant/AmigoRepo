using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.Mobile.ViewModel.Dialog
{
    public sealed class ConfirmTosViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IUserRepository _userRepository;

        public ConfirmTosViewModel(INavigator navigator, IUserRepository userRepository)
        {
            _navigator = navigator;
            _userRepository = userRepository;
            AcceptCommand = CreateCommand(AcceptTerms);
            CancelCommand = CreateCommand(Cancel);
        }

        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }
        private async Task AcceptTerms()
        {
            try
            {
                //Obtain user configuration
                var cfg = _userRepository.FindAll(x => x.ReadTos);
                if (!cfg.Any())
                {
                    var user = new BEUser() {Id = 0, ReadTos = true};
                   _userRepository.Add(user);                  
                }
                await _navigator.PopModalAsync();
            }
            catch (Exception)
            {
                await ShowError(ErrorCode.ToSAcceptTerms, AppString.errorCreatingUserProfile);
                throw;
            }
        }
        private async Task Cancel()
        {
            await MessageService.Current.ShowMessageAsync(AppString.titleTos, AppString.termsCancelMessage,AppString.btnDialogOk);
        }        
    }
}

