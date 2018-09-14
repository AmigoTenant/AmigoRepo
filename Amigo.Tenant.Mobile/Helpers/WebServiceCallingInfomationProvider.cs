using System;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Tasks.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Settings;
using XPO.ShuttleTracking.Mobile.Services;

namespace XPO.ShuttleTracking.Mobile.Helpers
{
    public class WebServiceCallingInfomationProvider : IWebServiceCallingInfomationProvider
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly INetworkInfoManager _networkInfoManager;
        public WebServiceCallingInfomationProvider(ISessionRepository sessionRepository,
            IActivityTypeRepository activityTypeRepository,
            INetworkInfoManager networkInfoManager)
        {
            _sessionRepository = sessionRepository;
            _activityTypeRepository = activityTypeRepository;
            _networkInfoManager = networkInfoManager;
        }

        public void FillTaskDefinition<T>(ref T task) where T : TaskDefinition
        {
            var session = _sessionRepository.GetSession();
            var dateTime = DateTime.Now;
            
            task.User = session.Username;
            task.Latitude = SessionParameter.Latitude;
            task.Longitude = SessionParameter.Longitude;
            task.Accuracy = SessionParameter.Accuracy; 
            task.LocationProvider = SessionParameter.LocationProvider;

            task.RegisteredDate = dateTime;
            task.ExecutionDate = dateTime;
        }

        public ShuttleTEventLogDTO FillTaskShuttleTEventLogDTO(string activityCode, DateTime dateTimeTemp)
        {
            char pipe = '|';
            var activity = _activityTypeRepository.FindByKey(activityCode);
            var version = InstanceConfigurationManager.Current.GetString(ServiceSettings.SemanticVersion);
            var atr = _networkInfoManager.GetVersion(version).Split(pipe);
            var session = _sessionRepository.GetSession();

            ShuttleTEventLogDTO shuttleTEventLogDTO = new ShuttleTEventLogDTO();
            shuttleTEventLogDTO.IsAutoDateTime = false;
            shuttleTEventLogDTO.IsSpoofingGPS = false;
            shuttleTEventLogDTO.IsRootedJailbreaked = false;
            shuttleTEventLogDTO.Platform = _networkInfoManager.GetSOName();
            shuttleTEventLogDTO.OSVersion = atr?[2];
            shuttleTEventLogDTO.AppVersion = atr?[0];
            shuttleTEventLogDTO.Latitude = SessionParameter.Latitude;
            shuttleTEventLogDTO.Longitude = SessionParameter.Longitude;
            shuttleTEventLogDTO.Accuracy = SessionParameter.Accuracy;
            shuttleTEventLogDTO.LocationProvider = SessionParameter.LocationProvider;
            shuttleTEventLogDTO.ReportedActivityTimeZone = TimeZoneInfo.Local.ToString();
            shuttleTEventLogDTO.ReportedActivityDate = dateTimeTemp;
            shuttleTEventLogDTO.ActivityTypeId = activity?.ActivityTypeId;
            return shuttleTEventLogDTO;
        }
    }
}
