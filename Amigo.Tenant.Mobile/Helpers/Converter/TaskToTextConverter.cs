using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;

namespace XPO.ShuttleTracking.Mobile.Helpers.Converter
{
    public class TaskToTextConverter: IValueConverter
    {
        private Dictionary<int,string> _texts = new Dictionary<int, string>
        {
            {StoreForwardCode.EVENT_STATUS_EXECUTING,  StoreForwardCode.EVENT_NAME_STATUS_EXECUTING},
            {StoreForwardCode.EVENT_STATUS_PENDING,    StoreForwardCode.EVENT_NAME_STATUS_PENDING},
            {StoreForwardCode.EVENT_STATUS_COMPLETED,  StoreForwardCode.EVENT_NAME_STATUS_COMPLETED},
            {StoreForwardCode.EVENT_STATUS_FAILED,     StoreForwardCode.EVENT_NAME_STATUS_FAILED}
          };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var task = value as TaskDefinition;
            if(task == null)return string.Empty;
            var status = StoreForwardCode.EVENT_STATUS_PENDING;
            if (task.Completed) status = StoreForwardCode.EVENT_STATUS_COMPLETED;
            else if (!task.Completed && task.ExecutionTimes > 1) status = StoreForwardCode.EVENT_STATUS_FAILED;
            return $"{_texts[status]}({task.ExecutionTimes})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
