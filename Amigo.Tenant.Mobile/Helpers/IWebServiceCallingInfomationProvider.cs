
using System;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;

namespace XPO.ShuttleTracking.Mobile.Helpers
{
    public interface IWebServiceCallingInfomationProvider
    {
        void FillTaskDefinition<T>(ref T task);
        ShuttleTEventLogDTO FillTaskShuttleTEventLogDTO(string activityCode, DateTime dateTimeTemp);
    }
}
