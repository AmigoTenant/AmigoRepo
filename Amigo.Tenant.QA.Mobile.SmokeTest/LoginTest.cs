using NUnit.Framework;
using Xamarin.UITest;
using XPO.ShuttleTracking.QA.Mobile.Common;
using XPO.ShuttleTracking.QA.Mobile.Common.Views;

namespace XPO.ShuttleTracking.QA.Mobile.SmokeTest
{
    [TestFixture(Platform.Android)]
    public class LoginTest : BaseSmokeTest
    {
        public LoginTest(Platform currentPlatform) : base(currentPlatform)
        {
            
        }

        [Test]
        public void TC1_Login()
        {
            LoginView loginView = new LoginView(this.CurrentApp);
            LicenseView licenseView = new LicenseView(this.CurrentApp);

            licenseView.AcceptTermOfServices(CurrentApp);
            loginView.LoginIntoApplication(Username, Password);
            MainMenuView mainMenuView = new MainMenuView(CurrentApp);
            mainMenuView.StartWorkday(Constants.YesNoAction.PressCancel);
            Assert.Pass();
        }

    }
}
