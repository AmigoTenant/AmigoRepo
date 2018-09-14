using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.View.Dialog
{
    public partial class ConfirmTosView : ContentPage
    {
        public ConfirmTosView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
