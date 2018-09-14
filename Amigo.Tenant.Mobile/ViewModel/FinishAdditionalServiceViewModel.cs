using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel.SearchItem;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class FinishAdditionalServiceViewModel: FinishViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IServiceRepository _serviceRepository;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;
        private int CommentMaxLenght = 50;

        private int timeDelay = 1000 * 1 * 1;
        private Timer timer;
        public FinishAdditionalServiceViewModel(INavigator navigator,
            IServiceRepository serviceRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            IEquipmentStatusRepository equipmentStatusRepository,
            IServiceTypeRepository serviceTypeRepository)
        {
            _navigator = navigator;
            _serviceRepository = serviceRepository;
            _infomationProvider = infomationProvider;
            _equipmentStatusRepository = equipmentStatusRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        public override void OnAppearing()
        {            
            Start = DateTime.ParseExact(GeneralService.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            if(timer == null)
                timer = new Timer(TimerTaskCallback, timeDelay);
        }

        private DateTime Start;
        public override void OnDisappearing()
        {            
            timer.Cancel();
            timer.Dispose();
            timer = null;
        }
        private void TimerTaskCallback()
        {
            TimeSpan span = DateTime.Now - Start;
            TimeElapsed = span.ToString(@"hh\:mm\:ss");
            OnPropertyChanged(nameof(TimeElapsed));
        }
        public override void OnPushed()
        {         
            base.OnPushed();
            if (!string.IsNullOrEmpty(GeneralService.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralService.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralService.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralService.CostCenterName;
            }
            else LblId = AppString.lblShipmentId.ToUpper();
            FromBlock = string.IsNullOrEmpty(GeneralService.StartName) ? NotRequired : GeneralService.StartName;

            MoveType = string.IsNullOrEmpty(GeneralService.MoveTypeDesc) ? NotRequired : GeneralService.MoveTypeDesc;
            LblTime = DateTime.ParseExact(GeneralService.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss);
            EquipmentNumber = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? NotRequired : GeneralService.EquipmentNumber;
            EquipmentTypeDesc = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? AppString.lblEquipmentType : GeneralService.EquipmentTypeDesc;
            _start = DateTime.ParseExact(GeneralService.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            TimeElapsed = "00:00:00";

            //Equipment Status
            StatusRequired = RequiredFieldValidator.RequiredField(_serviceTypeRepository.GetAll().ToList(), FieldRequirementCode.EqStatus, GeneralService.Service);
            if (StatusRequired == FieldRequirementCode.Value.Required || StatusRequired == FieldRequirementCode.Value.Optional)
            {
                var lstEqStatus = _equipmentStatusRepository.GetAll().OrderBy(x => x.Name).ToList();
                LstEquipmentStatus = lstEqStatus;
                selEquipmentStatus = GeneralService.EquipmentStatus;
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

        public ICommand GoToBlock => CreateCommand(async () =>
        {
            timer.Cancel();
            await _navigator.PushAsync<FromBlockSearchViewModel>(x => { x.GeneralService = GeneralService; x.AfterSelectItem = UpdateScreen; x.GeneralObjectType = (int)GeneralObject.Object.Service; });
        });
        public ICommand GoCancelSummaryAdditionalService => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogCancelService)) return;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await CancelService();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _serviceRepository.FindByKey(GeneralService.InternalId);
                        toUpdate.ServiceFinishDate = null;
                        toUpdate.CurrentState = MoveState.CancelMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.DriverComments = GeneralService.DriverComments;
                        _serviceRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationAdditionalServiceViewModel>(m => m.GeneralService = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });
        public ICommand GoSummaryAdditionalService => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (!await ShowYesNoAlert(AppString.lblDialogFinishService)) return;
            GeneralShuttletServiceDTO.ShipmentID = GeneralService.ShipmentID;
            GeneralShuttletServiceDTO.CostCenterId = GeneralService.CostCenter;
            GeneralShuttletServiceDTO.ShuttleTServiceId = GeneralService.ServiceId;
            GeneralShuttletServiceDTO.ServiceOrderNo = GeneralService.InternalId;
            GeneralShuttletServiceDTO.ServiceFinishDate = DateTimeOffset.Now;
            GeneralShuttletServiceDTO.ServiceFinishDateTZ = System.TimeZoneInfo.Local.ToString();
            GeneralShuttletServiceDTO.DriverComments = GeneralService.DriverComments;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await UpdateService();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        var toUpdate = _serviceRepository.FindByKey(GeneralService.InternalId);
                        toUpdate.ServiceFinishDate = GeneralShuttletServiceDTO.ServiceFinishDate.Value.ToString("O");
                        toUpdate.CurrentState = MoveState.FinishedMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.DriverComments = GeneralService.DriverComments;
                        _serviceRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationAdditionalServiceViewModel>(m => m.GeneralService = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });
        private Task<Infrastructure.BackgroundTasks.TaskStatus> CancelService()
        {
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.CancelService, DateTime.Now);

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

            CancelShuttleServiceRequest.ShuttleTServiceId = GeneralService.ServiceId;

            var request = new CancelMoveTaskDefinition
            {
                CancelShuttleServiceRequest = CancelShuttleServiceRequest,
            };
            _infomationProvider.FillTaskDefinition(ref request);

            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }

        private Task<Infrastructure.BackgroundTasks.TaskStatus> UpdateService()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.FinishService, dateTimeTemp);

            GeneralShuttletServiceDTO.EquipmentStatusId = GeneralService.EquipmentStatus;
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
            GeneralShuttletServiceDTO.ActivityTypeId = (int)shuttleTEventLogDTOTemp.ActivityTypeId;
            var request = new UpdateMoveTaskDefinition
            {
                GeneralService = GeneralService,
                ShuttletServiceDto = GeneralShuttletServiceDTO,
                ActivityCode = ActivityCode.FinishService
            };
            _infomationProvider.FillTaskDefinition(ref request);
            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }

        public ICommand RegisterStateBreak => CreateCommand<string>(activityCode =>
        {
            using (StartLoading(AppString.lblGeneralLoading))
            {
                RegisterLog(activityCode);
            }
        });
        private Infrastructure.BackgroundTasks.TaskStatus RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition();
            _infomationProvider.FillTaskDefinition(ref request);
            request.ActivityCode = activityCode;
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }
        private void UpdateScreen()
        {
            GeneralService = _serviceRepository.FindByKey(GeneralService.InternalId);
            OnPropertyChanged("GeneralService");
            Start = DateTime.ParseExact(GeneralService.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            timer = new Timer(TimerTaskCallback,timeDelay);
        }
        public ICommand CommentsCommand => CreateCommand(async () =>
        {
            var result = await PromptText(GeneralService.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralService.DriverComments = result.Text;
            _serviceRepository.Update(GeneralService);
            OnPropertyChanged("GeneralService");
        });

        private BEService _generalService;
        public BEService GeneralService
        {
            get { return _generalService; }
            set { SetProperty(ref _generalService, value); }
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
        private DateTime _start;

        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
        }
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
                GeneralService.EquipmentStatus = value;
                GeneralService.EquipmentStatusDesc = EquipmentStatus;
                _serviceRepository.Update(GeneralService);
            }
        }
        public override BEServiceBase GeneralServiceBase
        {
            get
            {
                return GeneralService;
            }
            set
            {
                if(!(value is BEService))throw new InvalidCastException("GeneralServiceBase");
                GeneralService = (BEService) value;
            }
        }
    }
}
