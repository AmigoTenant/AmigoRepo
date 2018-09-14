using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using static XPO.ShuttleTracking.QA.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class MainMenuView : BaseView
    {
        public MainMenuView(IApp app) : base(app)
        {

        }

        public void StartWorkday(YesNoAction action)
        {
            Func<AppQuery, AppQuery> ArriveButton = c => c.Marked("MainMenuArriveButton");
            Func<AppQuery, AppQuery> OkButton = c => c.Marked("button1");
            Func<AppQuery, AppQuery> CancelButton = c => c.Marked("button2");

            currentApp.WaitForElement(ArriveButton, "Arrive did not appear!", TimeSpan.FromSeconds(300));
            currentApp.Tap(ArriveButton);
            TapView(action == YesNoAction.PressYes ? OkButton : CancelButton);
        }

        public void TapAddMoveOption()
        {
            Func<AppQuery, AppQuery> RegisterMoveButton = c => c.Marked("RegisterMoveButton");
            TapView(RegisterMoveButton);
        }

        public void TapAcknowledge()
        {
            Func<AppQuery, AppQuery> RegisterMoveButton = c => c.Marked("Acknowledge");
            TapView(RegisterMoveButton);
        }

        public void TapAddServiceOption()
        {
            Func<AppQuery, AppQuery> RegisterMoveButton = c => c.Marked("Add Service");
            TapView(RegisterMoveButton);
        }

        public void TapStoreAndForward()
        {
            Func<AppQuery, AppQuery> StoreAndForwardOption = c => c.Marked("Store & Forward");
            currentApp.Tap(StoreAndForwardOption);
        }
    }
}
