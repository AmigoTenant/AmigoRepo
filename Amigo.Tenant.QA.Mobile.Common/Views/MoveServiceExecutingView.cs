using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class MoveServiceExecutingView : BaseView
    {
        public MoveServiceExecutingView(IApp app) : base(app)
        {
        }

        public void ConfirmFinishAction()
        {
            Func<AppQuery, AppQuery> FinishButton = c => c.Marked("FinishButton");
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button1");
            TapView(FinishButton);
            TapView(OkButton);
        }

        public void ConfirmFinishActionByText()
        {
            var button = currentApp.Query().First(x => !string.IsNullOrEmpty(x.Text) && x.Text.Contains("Finish Service"));
            Func<AppQuery, AppQuery> FinishButton = c => c.Marked(button.Text);
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button1");
            TapView(FinishButton);
            TapView(OkButton);
        }

        public void CancelFinishAction()
        {
            Func<AppQuery, AppQuery> FinishButton = c => c.Marked("FinishButton");
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button2");
            TapView(FinishButton);
            TapView(OkButton);
        }
        
    }
}
