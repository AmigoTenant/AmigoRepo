using System;
using System.ComponentModel;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class MainMenuView
    {
        private Color _colorOscuro, _colorClaro;
        public MainMenuView()
        {
            InitializeComponent();            
            _colorOscuro = (Color) Xamarin.Forms.Application.Current.Resources["ColorAlternateRows"];
            _colorClaro = (Color)Xamarin.Forms.Application.Current.Resources["ColorAlternateRowsB"];
            NavigationPage.SetHasBackButton(this, true);
        }        

        private MainMenuViewModel ViewModel => BindingContext as MainMenuViewModel;              
        protected override bool OnBackButtonPressed()
        {
            ViewModel.LogOutCommand.Execute(null);
            return true;
        }

        private static readonly string[] MenuOptions = new[]
        {
            AppString.btnToolbarSettings,
            AppString.btnToolbarTos,
            AppString.btnToolbarLogOut
        };
        private async void ShowActionSheet(object sender, EventArgs e)
        {
            var menuClicked = await DisplayActionSheet(null, AppString.btnToolbarCancel, null,MenuOptions);

            if (menuClicked == AppString.btnToolbarSettings)
            {
                ViewModel.SettingsCommand.Execute(null);
            }
            else if (menuClicked == AppString.btnToolbarTos)
            {
                ViewModel.TosCommand.Execute(null);
            }
            else if (menuClicked == AppString.btnToolbarLogOut)
            {
                ViewModel.LogOutCommand.Execute(null);
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            AlternateColors();
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            AlternateColors();
        }

        private bool _locked = false;
        private void AlternateColors()
        {
            if (_locked) return;
            _locked = true;

            Boolean ColorAlternate = true;
            foreach (var child in stkButtonList.Children)
            {
                try
                {
                    if (!child.IsVisible) continue;
                    child.BackgroundColor = ColorAlternate ? _colorOscuro : _colorClaro;
                    ColorAlternate = !ColorAlternate;
                }
                catch (Exception e)
                {
                    var ex = e.Message;
                }
            }
            _locked = false;
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!e.PropertyName.Equals("IsVisible"))
                AlternateColors();
        }
    }
}
