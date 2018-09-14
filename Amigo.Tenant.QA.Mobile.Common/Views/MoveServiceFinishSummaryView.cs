using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using static XPO.ShuttleTracking.QA.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class MoveServiceFinishSummaryView : BaseView
    {
        public MoveServiceFinishSummaryView(IApp app) : base(app)
        {
        }

        public void ContinueAction(MoveServiceAction action)
        {
            Func<AppQuery, AppQuery> ContinueButton = c => c.Marked("ContinueButton");
            Func<AppQuery, AppQuery> MoveButton = c => c.Marked("Move");
            Func<AppQuery, AppQuery> ServiceButton = c => c.Marked("Service");

            currentApp.WaitForElement(ContinueButton, "Arrive did not appear!", TimeSpan.FromSeconds(60));
            currentApp.Tap(ContinueButton);
            TapView(action == Constants.MoveServiceAction.PressMove ? MoveButton : ServiceButton);
        }


        public void GoToMainMenu()
        {
            Func<AppQuery, AppQuery> MainMenuButton = c => c.Marked("MainMenuButton");

            ScrollDownTo(MainMenuButton);
            TapView(MainMenuButton);
        }

        public void GoToMainMenuByText()
        {
            var button = currentApp.Query().First(x => !string.IsNullOrEmpty(x.Text) && x.Text.Contains("Main Menu"));
            Func<AppQuery, AppQuery> MainMenuButton = c => c.Marked(button.Text);

            ScrollDownTo(MainMenuButton);
            TapView(MainMenuButton);
        }

        public void StartNewMoveService(MoveServiceAction action)
        {
            Func<AppQuery, AppQuery> ArriveButton = c => c.Marked("StartNewButton");
            Func<AppQuery, AppQuery> MoveButton = c => c.Marked("Move");
            Func<AppQuery, AppQuery> ServiceButton = c => c.Marked("Service");

            currentApp.WaitForElement(ArriveButton, "Arrive did not appear!", TimeSpan.FromSeconds(60));
            currentApp.Tap(ArriveButton);
            TapView(action == Constants.MoveServiceAction.PressMove ? MoveButton : ServiceButton);
        }

        public void StartNewMoveServiceByText(MoveServiceAction action)
        {
            var button = currentApp.Query().First(x => !string.IsNullOrEmpty(x.Text) && x.Text.Contains("Start with"));
            Func<AppQuery, AppQuery> StartNewButton = c => c.Marked(button.Text);
            Func<AppQuery, AppQuery> MoveButton = c => c.Marked("Move");
            Func<AppQuery, AppQuery> ServiceButton = c => c.Marked("Service");

            currentApp.WaitForElement(StartNewButton, "Arrive did not appear!", TimeSpan.FromSeconds(100));
            currentApp.Tap(StartNewButton);
            TapView(action == Constants.MoveServiceAction.PressMove ? MoveButton : ServiceButton);
        }
        
    }
}
