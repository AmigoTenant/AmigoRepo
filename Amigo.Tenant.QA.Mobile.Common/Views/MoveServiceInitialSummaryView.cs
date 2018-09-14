using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class MoveServiceInitialSummaryView : BaseView
    {
        public MoveServiceInitialSummaryView(IApp app) : base(app)
        {
        }

        public void ConfirmStartAction()
        {
            Func<AppQuery, AppQuery> StartButton = c => c.Marked("StartButton");
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button1");

            TapView(StartButton);
            TapView(OkButton);
        }

        public void ConfirmStartActionByText()
        {
            var button = currentApp.Query().First(x => !string.IsNullOrEmpty(x.Text) && x.Text.Contains("Start Service"));
            Func<AppQuery, AppQuery> StartButton = c => c.Marked(button.Text);
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button1");
            TapView(StartButton);
            TapView(OkButton);
        }

        public void CancelStartAction()
        {
            Func<AppQuery, AppQuery> StartButton = c => c.Marked("StartButton");
            Func<AppQuery, AppQuery> CancelButton = c => c.Marked("button2");

            TapView(StartButton);
            TapView(CancelButton);
        }

    }
}
