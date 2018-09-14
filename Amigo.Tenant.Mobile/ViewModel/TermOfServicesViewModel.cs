using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Model;


namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class TermOfServicesViewModel: TodayViewModel
    {
        private readonly INavigator _navigator;

        public TermOfServicesViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }
    }
}
