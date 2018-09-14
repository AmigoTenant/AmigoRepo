using Xamarin.UITest;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class AddMoveByCostCenterView : AddMoveServiceBaseView
    {
        public AddMoveByCostCenterView(IApp app) : base(app)
        {
        }

        public void EnterChargeNumber(string chargeNumber)
        {
            //Func<AppQuery, AppQuery> ShipmentId = c => c.Marked("TxtShipmentId");
            //EnterTextView(ShipmentId, shipmentId);
        }
    }
}
