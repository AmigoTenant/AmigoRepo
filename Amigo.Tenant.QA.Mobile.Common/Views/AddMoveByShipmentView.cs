using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class AddMoveByShipmentView : AddMoveServiceBaseView
    {
        public AddMoveByShipmentView(IApp app) : base(app)
        {
            
        }

        public void EnterShipmentId(string shipmentId)
        {
            Func<AppQuery, AppQuery> ShipmentId = c => c.Marked("TxtShipmentId");
            EnterTextView(ShipmentId, shipmentId);
        }

        public void BackToMainMenu()
        {
            TapView(c => c.Class("ImageButton"));
        }

    }
}
