using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.ViewModel.SearchItem;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class FinishMoveViewModel : FinishViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;
        private int CommentMaxLenght = 50;

        private int timeDelay = 1000*1*1;
        private Timer timer;        

        public FinishMoveViewModel(INavigator navigator,
            IMoveRepository moveRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            IEquipmentStatusRepository equipmentStatusRepository,
            IServiceTypeRepository serviceTypeRepository,
            IEquipmentTypeRepository equipmentTypeRepository)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _infomationProvider = infomationProvider;
            _equipmentStatusRepository = equipmentStatusRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            ShowBobtailAuth = false;
        }

        public override void OnAppearing()
        {     
            if(timer == null)
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

        void TimerTaskCallback()
        {
            var span = DateTime.Now - _startTime;
            TimeElapsed = span.ToString(@"hh\:mm\:ss");
            OnPropertyChanged("TimeElapsed");
        }

        public override void OnPushed()
        {
            base.OnPushed();
            if (!string.IsNullOrEmpty(GeneralMove.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralMove.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralMove.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralMove.CostCenterName;
            }
            else
                LblId = AppString.lblShipmentId.ToUpper();

            FromBlock =         CheckEmpty(GeneralMove.StartName,       NotRequired);
            ToBlock =           CheckEmpty(GeneralMove.FinishName,      NotRequired);
            MoveType =          CheckEmpty(GeneralMove.MoveTypeDesc,    NotRequired);
            EquipmentNumber =   CheckEmpty(GeneralMove.EquipmentNumber, NotRequired);
            EquipmentTypeDesc = CheckEmpty(GeneralMove.EquipmentTypeDesc, AppString.lblEquipmentType);

            _startTime =    DateTime.ParseExact(GeneralMove.ServiceStartDate, "O", CultureInfo.InvariantCulture);
            StartTime =     _startTime.ToString(DateFormats.StandardHHmmss);
            TimeElapsed = "00:00:00";
            BobtailAuthorization = GeneralMove.Bobtail;

            //Equipment Status
            StatusRequired = RequiredFieldValidator.RequiredField(_serviceTypeRepository.GetAll().ToList(), FieldRequirementCode.EqStatus, GeneralMove.Service);
            var status = RequiredFieldValidator.RequiredField(_equipmentTypeRepository.GetAll().ToList(), FieldRequirementCode.EqStatus, GeneralMove.EquipmentType);
            StatusRequired = status ?? StatusRequired;
            if (StatusRequired == FieldRequirementCode.Value.Required || StatusRequired == FieldRequirementCode.Value.Optional)
            {
                var lstEqStatus = _equipmentStatusRepository.GetAll().OrderBy(x => x.Name).ToList();
                LstEquipmentStatus = lstEqStatus;
                selEquipmentStatus = GeneralMove.EquipmentStatus;
                EquipmentStatus = LstEquipmentStatus.FirstOrDefault(x => x.EquipmentStatusId == selEquipmentStatus).Name;
            }
        }

        private string CheckEmpty(string forcheck,string fallback)
        {
            return string.IsNullOrEmpty(forcheck) ? fallback : forcheck;
        }

        private void UpdateAfterBlock()
        {
            GeneralMove = _moveRepository.FindByKey(GeneralMove.InternalId);
            ToBlock = GeneralMove.FinishName;
        }

        #region Commands
        public ICommand ToBlockCommand => CreateCommand(async () =>
        {
            _moveRepository.Update(GeneralMove);
            await _navigator.PushAsync<ToBlockSearchViewModel>(x => { x.GeneralMove = GeneralMove; x.AfterSelectItem = UpdateAfterBlock; x.GeneralObjectType = (int)GeneralObject.Object.Move; });
        });
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
        public ICommand GoCancelMoveCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogCancelMove)) return;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await CancelMove();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        timer.Cancel();
                        var toUpdate = _moveRepository.FindByKey(GeneralMove.InternalId);
                        toUpdate.ServiceFinishDate = null;
                        toUpdate.CurrentState = MoveState.CancelMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.DriverComments = GeneralMove.DriverComments;
                        _moveRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationMoveViewModel>(m => m.GeneralMove = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });

        public ICommand GoResumeMoveCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogFinishMove)) return;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                var responseMove = await UpdateMove();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                        var toUpdate = _moveRepository.FindByKey(GeneralMove.InternalId);
                            //Se vuelve a llamar por si actualizó durante el S&F
                        toUpdate.ServiceFinishDate = GeneralShuttletServiceDTO.ServiceFinishDate.Value.ToString("O");
                        toUpdate.CurrentState = MoveState.FinishedMove;
                        toUpdate.CreationDate = DateTime.Today;
                        toUpdate.DriverComments = GeneralMove.DriverComments;
                        _moveRepository.Update(toUpdate);
                        SessionParameter.CurrentActivityChargeNo = string.Empty;
                        await _navigator.PushAsync<ConfirmationMoveViewModel>(m => m.GeneralMove = toUpdate);
                        ClearPendentMove();
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });

        public ICommand RegisterStateBreak => CreateCommand<string>((activityCode) =>
        {
            using (StartLoading(AppString.lblGeneralLoading))
            {
                RegisterLog(activityCode);
            }
        });

        private Infrastructure.BackgroundTasks.TaskStatus RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition {ActivityCode = activityCode};
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }

        private Task<Infrastructure.BackgroundTasks.TaskStatus> CancelMove()
        {
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.CancelMove, DateTime.Now);

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

            CancelShuttleServiceRequest.ShuttleTServiceId = GeneralMove.MoveId;

            var request = new CancelMoveTaskDefinition
            {
                CancelShuttleServiceRequest = CancelShuttleServiceRequest,
            };
            _infomationProvider.FillTaskDefinition(ref request);

            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }

        private Task<Infrastructure.BackgroundTasks.TaskStatus> UpdateMove()
        {            
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.FinishMove, DateTime.Now);

            GeneralShuttletServiceDTO.ShipmentID =          GeneralMove.ShipmentID;
            GeneralShuttletServiceDTO.CostCenterId =        GeneralMove.CostCenter;
            GeneralShuttletServiceDTO.ShuttleTServiceId =   GeneralMove.MoveId;
            GeneralShuttletServiceDTO.ServiceOrderNo =      GeneralMove.InternalId;
            GeneralShuttletServiceDTO.DriverComments =      GeneralMove.DriverComments;
            GeneralShuttletServiceDTO.EquipmentStatusId =   GeneralMove.EquipmentStatus;

            GeneralShuttletServiceDTO.IsAutoDateTime =      shuttleTEventLogDTOTemp.IsAutoDateTime;
            GeneralShuttletServiceDTO.IsSpoofingGPS =       shuttleTEventLogDTOTemp.IsSpoofingGPS;
            GeneralShuttletServiceDTO.IsRootedJailbreaked = shuttleTEventLogDTOTemp.IsRootedJailbreaked;
            GeneralShuttletServiceDTO.Platform =            shuttleTEventLogDTOTemp.Platform;
            GeneralShuttletServiceDTO.OSVersion =           shuttleTEventLogDTOTemp.OSVersion;
            GeneralShuttletServiceDTO.AppVersion =          shuttleTEventLogDTOTemp.AppVersion;
            GeneralShuttletServiceDTO.Latitude =            shuttleTEventLogDTOTemp.Latitude;
            GeneralShuttletServiceDTO.Longitude =           shuttleTEventLogDTOTemp.Longitude;
            GeneralShuttletServiceDTO.Accuracy =            shuttleTEventLogDTOTemp.Accuracy;
            GeneralShuttletServiceDTO.LocationProvider =    shuttleTEventLogDTOTemp.LocationProvider;
            GeneralShuttletServiceDTO.ReportedActivityTimeZone = shuttleTEventLogDTOTemp.ReportedActivityTimeZone;
            GeneralShuttletServiceDTO.ReportedActivityDate = shuttleTEventLogDTOTemp.ReportedActivityDate;
            GeneralShuttletServiceDTO.ActivityTypeId = (int) shuttleTEventLogDTOTemp.ActivityTypeId;

            GeneralShuttletServiceDTO.DestinationLocationId = string.IsNullOrEmpty(GeneralMove?.Finish)
                ? (int?)null
                : int.Parse(GeneralMove?.Finish);
            GeneralShuttletServiceDTO.ServiceFinishDate =   DateTimeOffset.Now;
            GeneralShuttletServiceDTO.ServiceFinishDateTZ = TimeZoneInfo.Local.ToString();
            
            var request = new UpdateMoveTaskDefinition
            {
                GeneralMove = GeneralMove,
                ShuttletServiceDto = GeneralShuttletServiceDTO,
                ActivityCode = ActivityCode.FinishMove,
            };
            _infomationProvider.FillTaskDefinition(ref request);

            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
        }

        public ICommand CommentsCommand => CreateCommand(async () =>
        {
            var result = await PromptText(GeneralMove.DriverComments,string.Format(AppString.lblCommentsMaxLength, CommentMaxLenght), CommentMaxLenght);
            if (!result.Ok) return;
            GeneralMove.DriverComments = result.Text;
            _moveRepository.Update(GeneralMove);
            OnPropertyChanged("GeneralMove");
        });

        #endregion

        private void UpdateScreen()
        {
            GeneralMove = _moveRepository.FindByKey(GeneralMove.InternalId);
            OnPropertyChanged("GeneralMove");
        }

        #region Variables

        private BEMove _generalMove;

        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
        }

        private string _lblToBlock;

        public string LblToBlock
        {
            get { return _lblToBlock; }
            set { SetProperty(ref _lblToBlock, value); }
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
            set { SetProperty(ref _lblNumber, value); }
        }

        private string _authorizationCode;

        public string AuthorizationCode
        {
            get { return _authorizationCode; }
            set { SetProperty(ref _authorizationCode, value); }
        }

        private string _equipmentNumber;

        public string EquipmentNumber
        {
            get { return _equipmentNumber; }
            set { SetProperty(ref _equipmentNumber, value); }
        }

        private string _bobtailAuthorization;

        public string BobtailAuthorization
        {
            get { return _bobtailAuthorization; }
            set { SetProperty(ref _bobtailAuthorization, value); }
        }

        private string _statusRequired;
        public string StatusRequired {
            get { return _statusRequired; }
            set { SetProperty(ref _statusRequired, value); }
        }

        private DateTime _startTime;
        private string _start;

        public string StartTime
        {
            get { return _start; }
            set { SetProperty(ref _start, value); }
        }

        private string _timeElapsed;
        public string TimeElapsed
        {
            get { return _timeElapsed; }
            set { SetProperty(ref _timeElapsed, value); }
        }

        private bool _showBobtailAuth;
        public bool ShowBobtailAuth
        {
            get { return _showBobtailAuth; }
            set { SetProperty(ref _showBobtailAuth, value); }
        }

        private string _toBlock;
        public string ToBlock
        {
            get { return _toBlock; }
            set { SetProperty(ref _toBlock, value); }
        }

        private string _fromBlock;
        public string FromBlock
        {
            get { return _fromBlock; }
            set { SetProperty(ref _fromBlock, value); }
        }
        private string _equipmentTypeDesc;
        public string EquipmentTypeDesc
        {
            get { return _equipmentTypeDesc; }
            set { SetProperty(ref _equipmentTypeDesc, value); }
        }
        private string _moveType;
        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
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
                GeneralMove.EquipmentStatus = value;
                GeneralMove.EquipmentStatusDesc = EquipmentStatus;
                _moveRepository.Update(GeneralMove);
            }
        }
        public override BEServiceBase GeneralServiceBase
        {
            get
            {
                return GeneralMove;
            }
            set
            {
                if (!(value is BEMove)) throw new InvalidCastException("GeneralServiceBase");

                GeneralMove = (BEMove)value;
            }
        }

        #endregion
    }
}
