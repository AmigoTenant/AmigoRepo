using Xamarin.UITest;

namespace XPO.ShuttleTracking.QA.Mobile.Common
{
    public class BaseTest
    {
        public IApp CurrentApp { get; set; }
        public Platform CurrentPlatform { get; set; }

        public BaseTest(Platform platform)
        {
            CurrentPlatform = platform;
        }

        public string Username
        {
            get { return "JGARCIA"; }

        }

        public string Password
        {
            get { return "xpo1234"; }

        }

    }
}