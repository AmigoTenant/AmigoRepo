using System;
using System.Globalization;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class StringToDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringToConvert = (string)value;
            if (string.IsNullOrEmpty(stringToConvert)) return string.Empty;
            try
            {
                var start = DateTime.ParseExact(stringToConvert, "O", CultureInfo.InvariantCulture);
                stringToConvert = start.ToString(DateFormats.MasterDataFormat);
                return stringToConvert;
            }
            catch (Exception ex){}
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
