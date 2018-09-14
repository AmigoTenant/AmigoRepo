using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class LoginView : BaseView
    {
        public LoginView(IApp app) : base(app)
        {
            
        }

        public void LoginIntoApplication(string user, string password)
        {
            Func<AppQuery, AppQuery> LoginButton = c => c.Marked("LoginBtnLogin");
            Func<AppQuery, AppQuery> UserField = c => c.Marked("LoginEntryDriverId");
            Func<AppQuery, AppQuery> PassField = c => c.Marked("LoginEntryPassword");

            EnterTextView(UserField, user);
            EnterTextView(PassField, password);
            Screenshot("LoginTask: tapping LogIn");
            TapView(LoginButton);
            Screenshot("LoginTask: Going to Main Menu");
        }
    }
}
