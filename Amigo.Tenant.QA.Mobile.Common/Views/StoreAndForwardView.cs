using Xamarin.UITest;

namespace XPO.ShuttleTracking.QA.Mobile.Common.Views
{
    public class StoreAndForwardView : BaseView
    {
        public StoreAndForwardView(IApp app) : base(app)
        {
        }

        public void BackToMainMenu()
        {
            TapView(c => c.Class("ImageButton"));
        }
    }
}
