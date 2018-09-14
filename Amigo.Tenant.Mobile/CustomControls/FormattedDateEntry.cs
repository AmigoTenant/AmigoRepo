using System;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.CustomControls
{
    public class FormattedDateEntry : Entry
    {
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(int), typeof(FormattedDateEntry), 0);

        public String Value
        {
            get { return (String)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool ShouldReactToTextChanges { get; set; }

        public FormattedDateEntry()
        {
            ShouldReactToTextChanges = true;
            this.TextChanged += OnCustomEntryTextChanged;
        }

        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue?.Length > 10)
                ((Entry)sender).Text = e.OldTextValue;
        }

    }
}
