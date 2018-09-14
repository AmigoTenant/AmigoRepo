using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;


namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class MoveEventCodeToTextConverter:IValueConverter
    {
        private Dictionary<int,string> _statusText = new Dictionary<int, string>
        {
            {0,"Correct"},
            {1,"Pending"},
            {2,"Incorrect"}       
        };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventCode = 0;
            if(!int.TryParse(value.ToString(),out eventCode))
            {
                return string.Empty;
            }

            var name = _statusText[eventCode];
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
