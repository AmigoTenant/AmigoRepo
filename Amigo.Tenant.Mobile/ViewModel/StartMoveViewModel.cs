using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class StartMoveViewModel : StartViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;

        private ShuttletServiceDTO _shuttletServiceDto;
        private BEMove _generalMove;
        private const string ChageTypeShipment = "S";
        private const string ChageTypeCostCenter = "C";
        private const int TimeDelay = 1000 * 1 * 1;
        private Timer _timer;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;
        

        public StartMoveViewModel(INavigator navigator,
            IMoveRepository moveRepository,            
            IActivityTypeRepository activityTypeRepository,
            IServiceTypeRepository serviceTypeRepository,
            ISessionRepository sessionRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _activityTypeRepository = activityTypeRepository;
            _sessionRepository = sessionRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if(_timer == null)
            {
                _timer = new Timer(TimerTaskCallback, TimeDelay);
            }            
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            _timer.Cancel();
            _timer.Dispose();
            _timer = null;
        }

        private void TimerTaskCallback()
        {
            LblTime = $"{DateTime.Now:HH:mm:ss}";
        }

        public override void OnPushed()
        {
            base.OnPushed();
            SaveMoveDto();
            ShowResult();
            FromBlock = string.IsNullOrEmpty(GeneralMove.StartName)? NotRequired: GeneralMove.StartName;
            ToBlock = string.IsNullOrEmpty(GeneralMove.FinishName) ? NotRequired : GeneralMove.FinishName;
            EquipmentTypeDesc = string.IsNullOrEmpty(GeneralMove.EquipmentNumber) ? AppString.lblEquipmentType: GeneralMove.EquipmentTypeDesc;
            MoveType = string.IsNullOrEmpty(GeneralMove.MoveTypeDesc)? NotRequired: GeneralMove.MoveTypeDesc;
            EquipmentNumber = string.IsNullOrEmpty(GeneralMove.EquipmentNumber)? NotRequired: GeneralMove.EquipmentNumber;
            Size = string.IsNullOrEmpty(GeneralMove.EquipmentSizeDesc)? NotRequired: GeneralMove.EquipmentSizeDesc;
            Status = string.IsNullOrEmpty(GeneralMove.EquipmentStatusDesc)? NotRequired: GeneralMove.EquipmentStatusDesc;
            ChassisNo = string.IsNullOrEmpty(GeneralMove.ChassisNumber)? NotRequired: GeneralMove.ChassisNumber;
            Product = string.IsNullOrEmpty(GeneralMove.ProductDescription)? NotRequired: GeneralMove.ProductDescription;
            BobtailAuthorization = GeneralMove.Bobtail;

            var lstAuthBobTail = _serviceTypeRepository.FindAll(m => m.ServiceTypeCode.Equals(ServiceTypeCode.Move) && m.Code.Equals(ServiceCode.AuthorizedBobtail));

            if (lstAuthBobTail.Any())
                ShowBobtailAuth = GeneralMove.MoveType == lstAuthBobTail.First().ServiceId;
            else
                ShowBobtailAuth = false;
            
        }

        #region Task
        private void ShowResult()
        {
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

            //OnPropertyChanged("LblId");
            //OnPropertyChanged("ChargeNo");
            //OnPropertyChanged("LblNumber");
        }
        private void SaveMoveDto()
        {
            if (GeneralMove == null) return;
            var moveDto = new ShuttletServiceDTO()
            {
                ServiceOrderNo = GeneralMove.InternalId,
                EquipmentNumber = GeneralMove?.EquipmentNumber,
                ServiceId = GeneralMove?.Service ?? 0,
                EquipmentTypeId = GeneralMove?.EquipmentType ?? 0,
                EquipmentStatusId = GeneralMove?.EquipmentStatus ?? 0,
                EquipmentSizeId = GeneralMove?.EquipmentSize ?? 0,
                DispatchingPartyId = GeneralMove?.DispatchingParty ?? 0,
                OriginLocationId = string.IsNullOrEmpty(GeneralMove?.Start) ? 0 : int.Parse(GeneralMove?.Start),
                DriverComments = GeneralMove?.DriverComments
            };
            if (!string.IsNullOrEmpty(GeneralMove?.Finish))
                moveDto.DestinationLocationId = int.Parse(GeneralMove?.Finish);

            if (string.IsNullOrEmpty(GeneralMove.Product))
            {
                moveDto.ProductId = null;
                moveDto.ProductDescription = null;
            }
            else
            {
                if (GeneralMove.Product.ToCharArray().All(Char.IsNumber))
                {
                    moveDto.ProductId = int.Parse(GeneralMove?.Product);
                    moveDto.ProductDescription = GeneralMove?.ProductDescription;
                }
                else if (GeneralMove.Product.ToCharArray().Any(Char.IsLetter))
                    moveDto.ProductDescription = GeneralMove?.Product;
            }

            var resultCode = _activityTypeRepository.FindByKey(ActivityCode.StartMove);
            if (resultCode != null) moveDto.ActivityTypeId = resultCode.ActivityTypeId;
            if (GeneralMove.CostCenter > 0)
            {
                moveDto.CostCenterId = GeneralMove.CostCenter;
                moveDto.ChargeType = ChageTypeCostCenter;
            }
            if (!string.IsNullOrEmpty(GeneralMove?.ShipmentID))
            {
                moveDto.ShipmentID = GeneralMove?.ShipmentID;
                moveDto.ChargeType = ChageTypeShipment;
            }
            string[] formats = { DateFormats.TankDateFormat };
            if (GeneralMove?.EquipmentTestDate25Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralMove?.EquipmentTestDate25Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate25Year = dateCon;
            }
            if (GeneralMove?.EquipmentTestDate5Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralMove?.EquipmentTestDate5Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate5Year = dateCon;
            }
            if (!string.IsNullOrEmpty(GeneralMove?.ChassisNumber)) moveDto.ChassisNumber = GeneralMove?.ChassisNumber;
            if (!string.IsNullOrEmpty(GeneralMove?.Bobtail)) moveDto.AuthorizationCode = GeneralMove?.Bobtail;
            _shuttletServiceDto = moveDto;
        }
        #endregion

        #region Commands
        public ICommand GoStopMoveCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection())
            {                
                return;
            }

            if (!await VerifyWorkday()) return;
            if (!await ShowYesNoAlert("", AppString.lblDialogStartMove)) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                _shuttletServiceDto.ServiceStartDate = DateTimeOffset.Now;
                _shuttletServiceDto.ServiceStartDateTZ = System.TimeZoneInfo.Local.ToString();
                SessionParameter.CurrentActivityChargeNo = LblNumber;

                var responseMove = await RegisterMove();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                        var toUpdate = _moveRepository.FindByKey(GeneralMove.InternalId);
                        toUpdate.CurrentState = MoveState.StartedMove;
                        toUpdate.ServiceStartDate = _shuttletServiceDto.ServiceStartDate.Value.ToString("O");
                        _moveRepository.Update(toUpdate);                       
                        await _navigator.PushAsync<FinishMoveViewModel>(m =>
                        {
                            m.GeneralMove = toUpdate;
                            m.GeneralShuttletServiceDTO = _shuttletServiceDto;
                        });
                        PersistPendentMove(State.Move(GeneralMove.InternalId,_shuttletServiceDto));
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
            }
        });
        private Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterMove()
        {
            var dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.StartMove, dateTimeTemp);
                      
            _shuttletServiceDto.IsAutoDateTime = shuttleTEventLogDTOTemp.IsAutoDateTime;
            _shuttletServiceDto.IsSpoofingGPS = shuttleTEventLogDTOTemp.IsSpoofingGPS;
            _shuttletServiceDto.IsRootedJailbreaked = shuttleTEventLogDTOTemp.IsRootedJailbreaked;
            _shuttletServiceDto.Platform = shuttleTEventLogDTOTemp.Platform;
            _shuttletServiceDto.OSVersion = shuttleTEventLogDTOTemp.OSVersion;
            _shuttletServiceDto.AppVersion = shuttleTEventLogDTOTemp.AppVersion;
            _shuttletServiceDto.Latitude = shuttleTEventLogDTOTemp.Latitude;
            _shuttletServiceDto.Longitude = shuttleTEventLogDTOTemp.Longitude;
            _shuttletServiceDto.Accuracy = shuttleTEventLogDTOTemp.Accuracy;
            _shuttletServiceDto.LocationProvider = shuttleTEventLogDTOTemp.LocationProvider;
            _shuttletServiceDto.ReportedActivityTimeZone = shuttleTEventLogDTOTemp.ReportedActivityTimeZone;
            _shuttletServiceDto.ReportedActivityDate = shuttleTEventLogDTOTemp.ReportedActivityDate;
            _shuttletServiceDto.ActivityTypeId = shuttleTEventLogDTOTemp.ActivityTypeId??0;
            _shuttletServiceDto.PayBy = _sessionRepository.GetSessionObject().TypeUser;

            var request = new RegisterMoveTaskDefinition
            {
                GeneralMove = GeneralMove,
                ActivityCode = ActivityCode.StartMove,
                ShuttletServiceDto = _shuttletServiceDto.Clone(),
            };
            _infomationProvider.FillTaskDefinition(ref request);
            return TaskManager.Current.TryExecuteOnline<ResponseDTO<int>>(request);
            //TaskManager.Current.RegisterStoreAndForward(request);
            //return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }

        public ICommand SettingsCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<SettingsViewModel>();
        });
        public ICommand HomeCommand => CreateCommand(async () =>
        {
            if (await ShowYesNoAlert(AppString.lblDialogDiscard))
            {
                await _navigator.PopToRootAsync();
            }
        });
        public ICommand RegisterStateBreak => CreateCommand<string>((activityCode) =>
        {
            using (StartLoading(AppString.lblGeneralLoading))
            {
                RegisterLog(activityCode);
            }
        });
        private async Task<bool> VerifyWorkday()
        {
            //Si no hay fechas previas, salir
            var session = _sessionRepository.GetSessionObject();
            if (string.IsNullOrEmpty(session.LastDateArrivalInfo)) return true;
            var lastStoredWorkday = DateTime.ParseExact(session.LastDateArrivalInfo, "O", CultureInfo.InvariantCulture); //Leer ultimo workday almacenada

            if (DateTime.Now.Date.CompareTo(lastStoredWorkday.Date) <= 0) return true;
            await ShowOkAlert(AppString.lblTooltipFinishPreviousWorkday);
            return false;
        }
        private Infrastructure.BackgroundTasks.TaskStatus RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition {ActivityCode = activityCode};
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }
        #endregion

        #region Variables
        public BEMove GeneralMove
        {
            get { return _generalMove; }
            set { SetProperty(ref _generalMove, value); }
        }

        private string _lblId;
        public string LblId
        {
            get { return _lblId; }
            set
            {
                SetProperty(ref _lblId, value);
            }
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
            set
            {
                SetProperty(ref _lblTime, value);
            }
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
        private string _equipmentNumber;
        public string EquipmentNumber
        {
            get { return _equipmentNumber; }
            set { SetProperty(ref _equipmentNumber, value); }
        }

        private string _equipmentTypeDesc;
        public string EquipmentTypeDesc
        {
            get { return _equipmentTypeDesc; }
            set { SetProperty(ref _equipmentTypeDesc, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _chassisNo;
        public string ChassisNo
        {
            get { return _chassisNo; }
            set { SetProperty(ref _chassisNo, value); }
        }

        private string _product;
        public string Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }

        private string _size;
        public string Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        private string _bobtailAuthorization;
        public string BobtailAuthorization
        {
            get { return _bobtailAuthorization; }
            set { SetProperty(ref _bobtailAuthorization, value); }
        }

        private string _moveType;
        public string MoveType
        {
            get { return _moveType; }
            set { SetProperty(ref _moveType, value); }
        }
        private bool _showBobtailAuth;
        public bool ShowBobtailAuth
        {
            get { return _showBobtailAuth; }
            set { SetProperty(ref _showBobtailAuth, value); }
        }
        
        #endregion
    }
}
