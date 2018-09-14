using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class LicenseView : BaseView
    {
        public LicenseView(IApp app) : base(app)
        {

        }

        public void AcceptTermOfServices(IApp app)
        {
            Func<AppQuery, AppQuery> ConfirmTOSButton = c => c.Marked("ConfirmTOSOK");
            app.WaitForElement(ConfirmTOSButton, "Term of Services never loaded ...", TimeSpan.FromSeconds(30));
            app.Tap(ConfirmTOSButton);
        }
        
    }
}
