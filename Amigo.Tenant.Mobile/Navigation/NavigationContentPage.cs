using System.Linq;
using TSI.Xamarin.Forms.Mvvm.ViewModels;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Navigation
{
    public class NavigationContentPage: ContentPage
    {        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = this.BindingContext as IViewModel;
            vm?.OnAppearing();
        }        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var vm = this.BindingContext as IViewModel;
            vm?.OnDisappearing();
        }        
    }
}
