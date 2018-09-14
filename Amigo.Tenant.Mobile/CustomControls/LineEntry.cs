using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class LineEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),typeof(Color),typeof(LineEntry),Color.Black);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
    }
}
