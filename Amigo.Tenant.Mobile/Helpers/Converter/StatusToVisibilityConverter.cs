using System;
using System.Globalization;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, null))
                return new GridLength(0);

            return string.IsNullOrWhiteSpace(value.ToString()) ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
