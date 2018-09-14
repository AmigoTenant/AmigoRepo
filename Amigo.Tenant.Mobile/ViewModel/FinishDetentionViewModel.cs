using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class FinishDetentionViewModel : FinishViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private int CommentMaxLenght = 50;

        private static readonly string NotRequired = AppString.lblMsgNotRequired;
        private int timeDelay = 1000 * 1 * 1;
        private Timer timer;
        public FinishDetentionViewModel(INavigator navigator,
            IDetentionRepository detentionRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            INetworkInfoManager networkInfoManager,
            IEquipmentStatusRepository equipmentStatusRepository,
            IServiceTypeRepository serviceTypeRepository)
        {
            _navigator = navigator;
            _detentionRepository = detentionRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
            _equipmentStatusRepository = equipmentStatusRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        public override void OnAppearing()
        {            
           _start = DateTime.ParseExact(GeneralDetention.ServiceStartDate, "O", CultureInfo.InvariantCulture);
           if(timer == null)
              timer = new Timer(TimerTaskCallback, timeDelay);
        }

        private DateTime _start;

        public override void OnDisappearing()
        {
            if (timer != null) {
                timer.Cancel();
                timer.Dispose();
                timer = null; }
        }
        private void TimerTaskCallback()
        {
            var span = DateTime.Now - _start;
            TimeElapsed = span.ToString(@"hh\:mm\:ss");
            OnPropertyChanged("TimeElapsed");
        }
        public override void OnPushed()
        {
            base.OnPushed();
            if (!string.IsNullOrEmpty(GeneralDetention.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralDetention.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralDetention.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralDetention.CostCenterName;
            }
            else LblId = AppString.lblShipmentId.ToUpper();
            FromBlock = string.IsNullOrEmpty(GeneralDetention.StartName) ? NotRequired : GeneralDetention.StartName;

            MoveType = string.IsNullOrEmpty(GeneralDetention.MoveTypeDesc) ? NotRequired : GeneralDetention.MoveTypeDesc;
            LblTime = DateTime.ParseExact(GeneralDetention.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss);
            EquipmentNumber = string.IsNullOrEmpty(GeneralDetention.EquipmentNumber) ? NotRequired : GeneralDetention.EquipmentNumber;
            EquipmentTypeDesc = string.IsNullOrEmpty(GeneralDetention.EquipmentNumber) ? AppString.lblEquipmentType : GeneralDetention.EquipmentTypeDesc;
            _start = DateTime.ParseExact(GeneralDetention.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            TimeElapsed = "00:00:00";

            //Equipment Status
            StatusRequired = RequiredFieldValidator.RequiredField(_serviceTypeRepository.GetAll().ToList(), FieldRequirementCode.EqStatus, GeneralDetention.Service);
            if (StatusRequired == FieldRequirementCode.Value.Required || StatusRequired == FieldRequirementCode.Value.Optional)
            {
                var lstEqStatus = _equipmentStatusRepository.GetAll().OrderBy(x => x.Name).ToList();
                LstEquipmentStatus = lstEqStatus;
                selEquipmentStatus = GeneralDetention.EquipmentStatus;
                EquipmentStatus = LstEquipmentStatus.FirstOrDefault(x => x.EquipmentStatusId == selEquipmentStatus).Name;
            }
        }
        public ICommand EquipmentStatusCommand => CreateCommand(async () =>
        {
            var options = LstEquipmentStatus.Select(x => x.Name).ToArray();
            var selectedOption =
                await
                    UserDialogs.Instance.ActionSheetAsync(
                        string.Format(AppString.lblDefaultSelection, AppString.lblStatus), AppString.btnDialogCancel,
                        null, cancelToken: null, buttons: options);

            selEquipmentStatus = LstEquipmentStatus.FirstOrDefault(x => x.Name == selectedOption).EquipmentStatusId;
        });

        public ICommand GoCancelDetention => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogCancelDetention)) return;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await CancelDetention();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _detentionRepository.FindByKey(GeneralDetention.InternalId);
                        toUpdate.CurrentState = MoveState.CancelMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.ServiceFinishDate = null;
                        toUpdate.DriverComments = GeneralDetention.DriverComments;
                        _detentionRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationDetentionViewModel>(m => m.GeneralDetention = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });

        public ICommand GoConfirmationDetention => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogFinishDetention)) return;

            GeneralShuttletServiceDTO.ShipmentID = GeneralDetention.ShipmentID;
            GeneralShuttletServiceDTO.CostCenterId = GeneralDetention.CostCenter;
            GeneralShuttletServiceDTO.ServiceFinishDate = DateTimeOffset.Now;
            GeneralShuttletServiceDTO.ServiceFinishDateTZ = System.TimeZoneInfo.Local.ToString();
            GeneralShuttletServiceDTO.ServiceOrderNo = GeneralDetention.InternalId;
            GeneralShuttletServiceDTO.DriverComments = GeneralDetention.DriverComments;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await UpdateDetention();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _detentionRepository.FindByKey(GeneralDetention.InternalId);
                        toUpdate.CurrentState = MoveState.FinishedMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.ServiceFinishDate = GeneralShuttletServiceDTO.ServiceFinishDate.Value.ToString("O");
                        toUpdate.DriverComments = GeneralDetention.DriverComments;
                        _detentionRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationDetentionViewModel>(m => m.GeneralDetention = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });
        private async Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition();
            _infomationProvider.FillTaskDefinition(ref request);
            request.ActivityCode = activityCode;
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }
        private Task<Infrastructure.BackgroundTasks.TaskStatus> CancelDetention()
        {
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.CancelDetention, DateTime.Now);

            CancelShuttleServiceRequest = new Application.DTOs.Requests.Tracking.CancelShuttleServiceRequest();
            CancelShuttleServiceRequest.IsAutoDateTime = shuttleTEventLogDTOTemp.IsAutoDateTime;
            CancelShuttleServiceRequest.IsSpoofingGPS = shuttleTEventLogDTOTemp.IsSpoofingGPS;
            CancelShuttleServiceRequest.IsRootedJailbreaked = shuttleTEventLogDTOTemp.IsRootedJailbreaked;
            CancelShuttleServiceRequest.Platform = shuttleTEventLogDTOTemp.Platform;
            CancelShuttleServiceRequest.OSVersion = shuttleTEventLogDTOTemp.OSVersion;
            CancelShuttleServiceRequest.AppVersion = shuttleTEventLogDTOTemp.AppVersion;
            CancelShuttleServiceRequest.Latitude = shuttleTEventLogDTOTemp.Latitude;
            CancelShuttleServiceRequest.Longitude = shuttleTEventLogDTOTemp.Longitude;
            CancelShuttleServiceRequest.Accuracy = shuttleTEventLogDTOTemp.Accuracy;
            CancelShuttleServiceRequest.LocationProvider = shuttleTEventLogDTOTemp.LocationProvider;
            CancelShuttleServiceRequest.ReportedActivityTimeZone = shuttleTEventLogDTOTemp.ReportedActivityTimeZone;
            CancelShuttleServiceRequest.ReportedActivityDate = shuttleTEventLogDTOTemp.ReportedActivityDate;
            CancelShuttleServiceRequest.ActivityTypeId = (int)shuttleTEventLogDTOTemp.ActivityTypeId;

            CancelShuttleServiceRequest.ShuttleTServiceId = GeneralDetention.DetentionId;

            var request = new CancelMoveTaskDefinition
            {
                CancelShuttleServiceRequest = CancelShuttleServiceRequest,
            };
            _infomationProvider.FillTaskDefinition(ref request);

            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }

        private Task<Infrastructure.BackgroundTasks.TaskStatus> UpdateDetention()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.FinishDetention, dateTimeTemp);

            GeneralShuttletServiceDTO.EquipmentStatusId = GeneralDetention.EquipmentStatus;
            GeneralShuttletServiceDTO.IsAutoDateTime = shuttleTEventLogDTOTemp.IsAutoDateTime;
            GeneralShuttletServiceDTO.IsSpoofingGPS = shuttleTEventLogDTOTemp.IsSpoofingGPS;
            GeneralShuttletServiceDTO.IsRootedJailbreaked = shuttleTEventLogDTOTemp.IsRootedJailbreaked;
            GeneralShuttletServiceDTO.Platform = shuttleTEventLogDTOTemp.Platform;
            GeneralShuttletServiceDTO.OSVersion = shuttleTEventLogDTOTemp.OSVersion;
            GeneralShuttletServiceDTO.AppVersion = shuttleTEventLogDTOTemp.AppVersion;
            GeneralShuttletServiceDTO.Latitude = shuttleTEventLogDTOTemp.Latitude;
            GeneralShuttletServiceDTO.Longitude = shuttleTEventLogDTOTemp.Longitude;
            GeneralShuttletServiceDTO.Accuracy = shuttleTEventLogDTOTemp.Accuracy;
            GeneralShuttletServiceDTO.LocationProvider = shuttleTEventLogDTOTemp.LocationProvider;
            GeneralShuttletServiceDTO.ReportedActivityTimeZone = shuttleTEventLogDTOTemp.ReportedActivityTimeZone;
            GeneralShuttletServiceDTO.ReportedActivityDate = shuttleTEventLogDTOTemp.ReportedActivityDate;
            GeneralShuttletServiceDTO.ServiceFinishDate = DateTimeOffset.Now;
            GeneralShuttletServiceDTO.ServiceFinishDateTZ = TimeZoneInfo.Local.ToString();
            GeneralShuttletServiceDTO.ActivityTypeId = (int)shuttleTEventLogDTOTemp.ActivityTypeId;
            var request = new UpdateMoveTaskDefinition
            {
                GeneralDetention = GeneralDetention,
                ShuttletServiceDto = GeneralShuttletServiceDTO,
                ActivityCode = ActivityCode.FinishDetention
            };
            _infomationProvider.FillTaskDefinition(ref request);
            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }
        private void UpdateScreen()
        {
            GeneralDetention = _detentionRepository.FindByKey(GeneralDetention.InternalId);
            OnPropertyChanged("GeneralDetention");
            _start = DateTime.ParseExact(GeneralDetention.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            if(timer == null)
            {
                timer = new Timer(TimerTaskCallback, timeDelay);
            }            
        }
        public ICommand CommentsCommand => CreateCommand(async () =>
        {
            var result = await PromptText(GeneralDetention.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralDetention.DriverComments = result.Text;
            _detentionRepository.Update(GeneralDetention);
            OnPropertyChanged("GeneralDetention");
        });
        private string _statusRequired;
        public string StatusRequired
        {
            get { return _statusRequired; }
            set { SetProperty(ref _statusRequired, value); }
        }
        private string _equipmentStatus;
        public string EquipmentStatus
        {
            get { return _equipmentStatus; }
            set { SetProperty(ref _equipmentStatus, value); }
        }
        private IEnumerable<EquipmentStatusDTO> _lstEquipmentStatus;
        public IEnumerable<EquipmentStatusDTO> LstEquipmentStatus
        {
            get { return _lstEquipmentStatus; }
            set { SetProperty(ref _lstEquipmentStatus, value); }
        }
        private int _selEquipmentStatus;
        public int selEquipmentStatus
        {
            get { return _selEquipmentStatus; }
            set
            {
                if (_selEquipmentStatus == value) return;
                SetProperty(ref _selEquipmentStatus, value);
                EquipmentStatus = LstEquipmentStatus.FirstOrDefault(x => x.EquipmentStatusId == value).Name;
                GeneralDetention.EquipmentStatus = value;
                GeneralDetention.EquipmentStatusDesc = EquipmentStatus;
                _detentionRepository.Update(GeneralDetention);
            }
        }

        private BEDetention _generalDetention;
        public BEDetention GeneralDetention
        {
            get { return _generalDetention; }
            set { SetProperty(ref _generalDetention, value); }
        }

        private string _lblFromBlock;
        public string LblFromBlock
        {
            get { return _lblFromBlock; }
            set
            {
                SetProperty(ref _lblFromBlock, value);
            }
        }
        private string _fromBlock;
        public string FromBlock
        {
            get { return _fromBlock; }
            set { SetProperty(ref _fromBlock, value); }
        }
        private string _lblId;
        public string LblId
        {
            get { return _lblId; }
            set { SetProperty(ref _lblId, value); }
        }
        private string _lblNumber;
        public string LblNumber
        {
            get { return _lblNumber; }
            set
            {
                SetProperty(ref _lblNumber, value);
            }
        }

        private string _lblTime;
        public string LblTime
        {
            get { return _lblTime; }
            set { SetProperty(ref _lblTime, value); }
        }
        private string _equipmentTypeDesc;
        public string EquipmentTypeDesc
        {
            get { return _equipmentTypeDesc; }
            set { SetProperty(ref _equipmentTypeDesc, value); }
        }
        private string _timeElapsed;
        public string TimeElapsed
        {
            get { return _timeElapsed; }
            set { SetProperty(ref _timeElapsed, value); }
        }
        private string _equipmentNumber;
        public string EquipmentNumber
        {
            get { return _equipmentNumber; }
            set { SetProperty(ref _equipmentNumber, value); }
        }
        private bool _showAuthorization;
        public bool ShowAuthorization
        {
            get { return _showAuthorization; }
            set { SetProperty(ref _showAuthorization, value); }
        }

        private string _moveType;

        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
        }
        public override BEServiceBase GeneralServiceBase
        {
            get
            {
                return GeneralDetention;
            }
            set
            {
                if (!(value is BEDetention)) throw new InvalidCastException("GeneralServiceBase");

                GeneralDetention = (BEDetention)value;
            }
        }
    }
}
