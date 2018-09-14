using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class AddServiceByShipmentView : AddMoveServiceBaseView
    {
        public AddServiceByShipmentView(IApp app) : base(app)
        {
        }

        public void EnterShipmentId(string shipmentId)
        {
            Func<AppQuery, AppQuery> ShipmentId = c => c.Marked("TxtShipmentId");
            EnterTextView(ShipmentId, shipmentId);
        }

        public void EnterShipmentIdByText(string shipmentId)
        {
            
        }
    }
}
