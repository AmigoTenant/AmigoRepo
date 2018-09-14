using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace XPO.ShuttleTracking.QA.Mobile.Common
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .InstalledApp("XPO.ShuttleTracking.Mobile.Droid")
                    .EnableLocalScreenshots()
                    .StartApp(AppDataMode.Clear);
            }

            return ConfigureApp
                .iOS
                .StartApp();
        }
    }
}
