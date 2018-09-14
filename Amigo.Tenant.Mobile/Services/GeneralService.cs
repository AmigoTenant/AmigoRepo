using Plugin.Connectivity;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;

namespace XPO.ShuttleTracking.Mobile.Services
{
    public class GeneralService : IGeneralService
    {
        public bool IsConnectivity()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}
