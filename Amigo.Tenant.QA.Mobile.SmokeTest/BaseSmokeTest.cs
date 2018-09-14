using NUnit.Framework;
using Xamarin.UITest;
using XPO.ShuttleTracking.QA.Mobile.Common;

namespace XPO.ShuttleTracking.QA.Mobile.SmokeTest
{
    public class BaseSmokeTest : BaseTest
    {
        public BaseSmokeTest(Platform platform) : base(platform)
        {
            
        }

        [SetUp]
        public void BeforeEachTest()
        {
            CurrentApp = AppInitializer.StartApp(CurrentPlatform);
        }
    }
}
