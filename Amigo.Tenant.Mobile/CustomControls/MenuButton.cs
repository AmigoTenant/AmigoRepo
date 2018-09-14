using System.Windows.Input;
using ScnViewGestures.Plugin.Forms;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class MenuButton : StackLayout
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<MenuButton, ICommand>(p => p.Command, null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<MenuButton, object>(p => p.CommandParameter, null);
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public MenuButton()
        {
            #region Gesture 1

            var titleTap = new Label {Style = (Style)Xamarin.Forms.Application.Current.Resources["MenuLabelBullet"]};

            var pressLabel = new Label
            {
                Text = "Prueba",
                Style = (Style)Xamarin.Forms.Application.Current.Resources["MenuLabelText"]
            };

            var tapViewGestures = new ViewGestures
            {
                BackgroundColor = Color.Transparent,
                Content = pressLabel,
                AnimationEffect = ViewGestures.AnimationType.atScaling,
                AnimationScale = -10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            tapViewGestures.Tap +=
                (s, e) =>
                {
                    Command.Execute(CommandParameter);
                    //Application.Current.MainPage.DisplayAlert("Tap", "Gesture finished", "OK"); 
                };

            #endregion

            this.Style = (Style)Xamarin.Forms.Application.Current.Resources["MenuStackDrawer"];
            this.Children.Add(tapViewGestures);
            this.Children.Add(titleTap);
        }
        
    }
}
