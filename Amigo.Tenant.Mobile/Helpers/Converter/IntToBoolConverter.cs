using System;
using System.Globalization;
using Xamarin.Forms;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
	public class IntToBoolConverter : IValueConverter
	{

		public int TrueValue
		{
			get;
			set;
		} = 0;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(TrueValue);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
