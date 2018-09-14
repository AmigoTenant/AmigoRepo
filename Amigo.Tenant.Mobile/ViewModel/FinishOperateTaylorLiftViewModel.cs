using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
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
    public class FinishOperateTaylorLiftViewModel : FinishViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;
        private int CommentMaxLenght = 50;

        private int timeDelay = 1000 * 1 * 1;
        private Timer timer;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;

        public FinishOperateTaylorLiftViewModel(INavigator navigator,
             IOperateTaylorLiftRepository operateTaylorLiftRepository,
             IWebServiceCallingInfomationProvider infomationProvider,
             INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
        }
        public override void OnAppearing()
        {            
            _start = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            if (timer == null)
                timer = new Timer(TimerTaskCallback, timeDelay);       
        }

        public override void OnDisappearing()
        {
            if (timer != null) {

                timer.Cancel();
                timer.Dispose();
                timer = null;
            }
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
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralOperateTaylorLift.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralOperateTaylorLift.CostCenterName;
            }
            else LblId = AppString.lblShipmentId.ToUpper();
            FromBlock = string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName) ? NotRequired : GeneralOperateTaylorLift.StartName;

            MoveType = string.IsNullOrEmpty(GeneralOperateTaylorLift.MoveTypeDesc) ? NotRequired : GeneralOperateTaylorLift.MoveTypeDesc;
            LblTime = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture).ToString(DateFormats.StandardHHmmss);
            EquipmentNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? NotRequired : GeneralOperateTaylorLift.EquipmentNumber;
            EquipmentTypeDesc = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? AppString.lblEquipmentType : GeneralOperateTaylorLift.EquipmentTypeDesc;
            _start = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            TimeElapsed = "00:00:00";
        }
        public ICommand GoCancelOperateTaylorLift => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogCancelOperate)) return;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await CancelTaylorLift();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _operateTaylorLiftRepository.FindByKey(GeneralOperateTaylorLift.InternalId);
                        toUpdate.CurrentState = MoveState.CancelMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.ServiceFinishDate = null;
                        toUpdate.DriverComments = GeneralOperateTaylorLift.DriverComments;
                        _operateTaylorLiftRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationOperateTaylorLiftViewModel>(m => m.GeneralOperateTaylorLift = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });

        public ICommand GoConfirmationOperateTaylorLift => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (!await ShowYesNoAlert(AppString.lblDialogFinishOperateTaylorLift)) return;
            GeneralShuttletServiceDTO.ShipmentID = GeneralOperateTaylorLift.ShipmentID;
            GeneralShuttletServiceDTO.CostCenterId = GeneralOperateTaylorLift.CostCenter;
            GeneralShuttletServiceDTO.ShuttleTServiceId = GeneralOperateTaylorLift.OperateTaylorLiftId;
            //GeneralShuttletServiceDTO.DestinationLocationId = int.Parse(GeneralService?.Finish); //Todo: Por qué lo sacaron?
            GeneralShuttletServiceDTO.ServiceOrderNo = GeneralOperateTaylorLift.InternalId;
            GeneralShuttletServiceDTO.ServiceFinishDate = DateTimeOffset.Now;
            GeneralShuttletServiceDTO.ServiceFinishDateTZ = System.TimeZoneInfo.Local.ToString();
            GeneralShuttletServiceDTO.DriverComments = GeneralOperateTaylorLift.DriverComments;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await UpdateTaylorLift();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _operateTaylorLiftRepository.FindByKey(GeneralOperateTaylorLift.InternalId);
                        toUpdate.CurrentState = MoveState.FinishedMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.ServiceFinishDate = GeneralShuttletServiceDTO.ServiceFinishDate.Value.ToString("O");
                        toUpdate.DriverComments = GeneralOperateTaylorLift.DriverComments;
                        _operateTaylorLiftRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationOperateTaylorLiftViewModel>(m => m.GeneralOperateTaylorLift = toUpdate);
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
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }
        private Task<Infrastructure.BackgroundTasks.TaskStatus> CancelTaylorLift()
        {
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.CancelTaylorLift, DateTime.Now);

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

            CancelShuttleServiceRequest.ShuttleTServiceId = GeneralOperateTaylorLift.OperateTaylorLiftId;

            var request = new CancelMoveTaskDefinition
            {
                CancelShuttleServiceRequest = CancelShuttleServiceRequest,
            };
            _infomationProvider.FillTaskDefinition(ref request);

            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }
        private Task<Infrastructure.BackgroundTasks.TaskStatus> UpdateTaylorLift()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.FinishTaylorLift, dateTimeTemp);

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
                GeneralOperateTaylorLift = GeneralOperateTaylorLift,
                ShuttletServiceDto = GeneralShuttletServiceDTO,
                ActivityCode = ActivityCode.FinishTaylorLift
            };

            _infomationProvider.FillTaskDefinition(ref request);
            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }
        public ICommand CommentsCommand => CreateCommand(async () =>
        {
            var result = await PromptText(GeneralOperateTaylorLift.DriverComments, string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralOperateTaylorLift.DriverComments = result.Text;
            _operateTaylorLiftRepository.Update(GeneralOperateTaylorLift);
            OnPropertyChanged("GeneralOperateTaylorLift");
        });
        private void UpdateScreen()
        {
            GeneralOperateTaylorLift = _operateTaylorLiftRepository.FindByKey(GeneralOperateTaylorLift.InternalId);
            OnPropertyChanged("GeneralOperateTaylorLift");
            _start = DateTime.ParseExact(GeneralOperateTaylorLift.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            timer = new Timer(TimerTaskCallback,timeDelay);
        }
        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
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
        public override BEServiceBase GeneralServiceBase
        {
            get
            {
                return GeneralOperateTaylorLift;
            }
            set
            {
                if (!(value is BEOperateTaylorLift)) throw new InvalidCastException("GeneralServiceBase");

                GeneralOperateTaylorLift = (BEOperateTaylorLift)value;
            }
        }
    }
}
