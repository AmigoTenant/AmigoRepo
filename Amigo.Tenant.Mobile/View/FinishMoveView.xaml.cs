using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.View.Abstract;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class FinishMoveView : NavigatingPage
    {        
        public FinishMoveView()
        {
            InitializeComponent();            
        }        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);            
            Circular.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Circular.Stop();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}
