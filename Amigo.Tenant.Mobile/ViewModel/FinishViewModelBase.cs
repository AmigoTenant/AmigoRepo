using Newtonsoft.Json;
using TSI.Xamarin.Forms.Persistence.Storage;
using XPO.ShuttleTracking.Application.DTOs.Requests.Tracking;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;
using XPO.ShuttleTracking.Mobile.Model;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public abstract class FinishViewModelBase : TodayViewModel
    {
        // ReSharper disable once InconsistentNaming to avoid refactoring legacy code in Move's ViewModels
        protected ShuttletServiceDTO _generalShuttletServiceDto;
        protected CancelShuttleServiceRequest _cancelShuttleServiceRequest;
        public abstract BEServiceBase GeneralServiceBase { get; set; }

        public virtual ShuttletServiceDTO GeneralShuttletServiceDTO
        {
            get { return _generalShuttletServiceDto; }
            set
            {
                _generalShuttletServiceDto = value;
                SetProperty(ref _generalShuttletServiceDto, value);
            }
        }

        public virtual CancelShuttleServiceRequest CancelShuttleServiceRequest
        {
            get { return _cancelShuttleServiceRequest; }
            set { SetProperty(ref _cancelShuttleServiceRequest, value);}
        }

        protected readonly IPersistentStorageManager PersistentStorageManager;

        protected FinishViewModelBase()
        {
            PersistentStorageManager = ((App)Xamarin.Forms.Application.Current).Resolver.Resolve<IPersistentStorageManager>();
        }

        protected void ClearPendentMove()
        {
            PersistentStorageManager.RemoveValue(SecuritySettings.PendentMoveKey);
        }
    }

    public abstract class StartViewModelBase : TodayViewModel
    {
        protected readonly IPersistentStorageManager PersistentStorageManager;

        protected StartViewModelBase()
        {
            PersistentStorageManager = ((App)Xamarin.Forms.Application.Current).Resolver.Resolve<IPersistentStorageManager>();
        }

        protected void PersistPendentMove(State state)
        {
            var json = JsonConvert.SerializeObject(state);
            PersistentStorageManager.AddValue(SecuritySettings.PendentMoveKey,json);
        }
    }
}