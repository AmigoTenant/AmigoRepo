using System;
using System.Globalization;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class TaskStatusToColorConverter : IValueConverter
    {
        public Color FailedColor { get; set; }
        public Color ValidColor { get; set; }
        public static readonly Color DefaultColor = Color.Black;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var task = value as TaskDefinition;
            if (task == null) return DefaultColor;

            return task.Completed ? FailedColor : ValidColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}