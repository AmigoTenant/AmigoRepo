using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class AddServiceByCostCenterView : AddMoveServiceBaseView
    {
        public AddServiceByCostCenterView(IApp app) : base(app)
        {
        }

        public void EnterChargeNo(string chargeNo)
        {
            Func<AppQuery, AppQuery> CostCenter = c => c.Marked("TxtCostCenter");
            Func<AppQuery, AppQuery> SelectedChargeNo = c => c.Class("FormsTextView").Text(chargeNo);
            TapView(CostCenter);
            TapView(SelectedChargeNo);
        }
    }
}
