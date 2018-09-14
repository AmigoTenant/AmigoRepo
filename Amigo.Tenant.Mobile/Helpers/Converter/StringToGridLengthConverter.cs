using System;
using System.Globalization;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class StringToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value?.ToString()) ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
