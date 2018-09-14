using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class ItemsFoundToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            var objeto = (IList)value;
            return (objeto.Count <= 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
