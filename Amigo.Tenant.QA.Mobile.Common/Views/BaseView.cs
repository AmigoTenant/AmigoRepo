using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class BaseView
    {
        public IApp currentApp { get; set; }

        public BaseView(IApp app)
        {
            currentApp = app;
        }

        public void EnterTextView(Func<AppQuery, AppQuery> view, string text, string timeOutMessage = "Error entering text!", int timeOut = 180)
        {
            if (string.IsNullOrEmpty(text)) return;
            currentApp.WaitForElement(view, timeOutMessage, timeOut.Equals(0) ? null : (TimeSpan?)TimeSpan.FromSeconds(timeOut));
            currentApp.EnterText(view, text);
            currentApp.DismissKeyboard();
        }

        public void TapView(Func<AppQuery, AppQuery> view, string timeOutMessage = "Error in tap view!", int timeOut = 180)
        {
            currentApp.WaitForElement(view, timeOutMessage, timeOut.Equals(0) ? null: (TimeSpan?) TimeSpan.FromSeconds(timeOut));
            currentApp.Tap(view);
        }

        public void ScrollDownTo(Func<AppQuery, AppQuery> view)
        {
            currentApp.ScrollDownTo(view);
        }

        public void Screenshot(string title)
        {
            currentApp.Screenshot(title);
        }

        public void CallRepl()
        {
            currentApp.Repl();
        }
    }
}
