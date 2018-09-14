using System;
using System.Globalization;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class RequiredToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() != FieldRequirementCode.Value.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
