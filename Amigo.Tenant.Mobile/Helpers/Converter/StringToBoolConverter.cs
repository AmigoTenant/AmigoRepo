using System;
using System.Globalization;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value == null ? string.Empty : value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
