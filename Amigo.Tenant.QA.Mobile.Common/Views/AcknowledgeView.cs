using Xamarin.UITest;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class AcknowledgeView : BaseView
    {
        public AcknowledgeView(IApp app) : base(app)
        {
        }

        public void EnterChargeNo(string chargeNo)
        {
            TapView(c => c.Text("Select Charge No"));
            TapView(c => c.Text(chargeNo));
        }

        public void ToggleSwitch()
        {
            TapView(c => c.Class("SwitchRenderer"));
        }

        public void Authorize(string autorizeCode)
        {
            EnterTextView(c => c.Class("EntryEditText"), autorizeCode);
        }

        public void Approve()
        {
            TapView(c => c.Text("Approve"));
            TapView(c => c.Text("Accept"));
        }
    }
}
