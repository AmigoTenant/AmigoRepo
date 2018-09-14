using System;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using System.Globalization;
using System.Linq;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class StartOperateTaylorLiftViewModel : StartViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly ISessionRepository _sessionRepository;
        private ShuttletServiceDTO _shuttletServiceDto;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;

        private const string ChageTypeShipment = "S";
        private const string ChageTypeCostCenter = "C";
        private int timeDelay = 1000 * 1 * 1;
        private Timer _timer;
        private static readonly string NotRequired = AppString.lblMsgNotRequired;

        public StartOperateTaylorLiftViewModel(INavigator navigator,
            IOperateTaylorLiftRepository operateTaylorLiftRepository,
            IActivityTypeRepository activityTypeRepository,
            ISessionRepository sessionRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _activityTypeRepository = activityTypeRepository;
            _sessionRepository = sessionRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if(_timer == null)
            {
                _timer = new Timer(TimerTaskCallback, timeDelay);
            }            
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            _timer.Cancel();
            _timer.Dispose();
            _timer = null;
        }
        public override void OnPushed()
        {
            base.OnPushed();
            SaveMoveDto();
            ShowResult();
        }
        #region Navigation
        private void TimerTaskCallback()
        {
            LblTime = $"{DateTime.Now:HH:mm:ss}";
            OnPropertyChanged("LblTime");
        }
        private void ShowResult()
        {
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralOperateTaylorLift.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralOperateTaylorLift.CostCenterName;
            }

            TxtBlock = string.IsNullOrEmpty(GeneralOperateTaylorLift.StartName) ? NotRequired : GeneralOperateTaylorLift.StartName;
            TxtH34 = GeneralOperateTaylorLift.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
            TxtServiceType = string.IsNullOrEmpty(GeneralOperateTaylorLift.MoveTypeDesc) ? NotRequired : GeneralOperateTaylorLift.MoveTypeDesc;
            LblEquipmentType = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? AppString.lblEquipmentType : GeneralOperateTaylorLift.EquipmentTypeDesc;
            TxtEquipmentNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentNumber) ? NotRequired : GeneralOperateTaylorLift.EquipmentNumber;
            TxtEquipmentSize = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentSizeDesc) ? NotRequired : GeneralOperateTaylorLift.EquipmentSizeDesc;
            TxtEquipmentStatus = string.IsNullOrEmpty(GeneralOperateTaylorLift.EquipmentStatusDesc) ? NotRequired : GeneralOperateTaylorLift.EquipmentStatusDesc;
            TxtChassisNumber = string.IsNullOrEmpty(GeneralOperateTaylorLift.ChassisNumber) ? NotRequired : GeneralOperateTaylorLift.ChassisNumber;
            TxtProduct = string.IsNullOrEmpty(GeneralOperateTaylorLift.ProductDescription) ? NotRequired : GeneralOperateTaylorLift.ProductDescription;

            OnPropertyChanged("LblId");
            OnPropertyChanged("TxtBlock");
            OnPropertyChanged("TxtH34");
            OnPropertyChanged("TxtServiceType");
            OnPropertyChanged("LblEquipmentType");
            OnPropertyChanged("TxtEquipmentNumber");
            OnPropertyChanged("TxtEquipmentSize");
            OnPropertyChanged("TxtEquipmentStatus");
            OnPropertyChanged("ChargeNo");
            OnPropertyChanged("LblNumber");
        }
        private void SaveMoveDto()
        {
            if (GeneralOperateTaylorLift == null) return;

            var moveDto = new ShuttletServiceDTO()
            {
                ServiceOrderNo = GeneralOperateTaylorLift.InternalId,
                EquipmentNumber = GeneralOperateTaylorLift?.EquipmentNumber,
                ServiceId = GeneralOperateTaylorLift?.Service ?? 0,
                EquipmentTypeId = GeneralOperateTaylorLift?.EquipmentType ?? 0,
                EquipmentStatusId = GeneralOperateTaylorLift?.EquipmentStatus ?? 0,
                EquipmentSizeId = GeneralOperateTaylorLift?.EquipmentSize ?? 0,
                DispatchingPartyId = GeneralOperateTaylorLift?.DispatchingParty ?? 0,
                OriginLocationId = string.IsNullOrEmpty(GeneralOperateTaylorLift?.Start) ? 0 : int.Parse(GeneralOperateTaylorLift?.Start),
                HasH34 = GeneralOperateTaylorLift?.HasH34,
                DriverComments = GeneralOperateTaylorLift?.DriverComments
            };
            
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.Finish))
                moveDto.DestinationLocationId = int.Parse(GeneralOperateTaylorLift?.Finish);

            if (string.IsNullOrEmpty(GeneralOperateTaylorLift.Product))
            {
                moveDto.ProductId = null;
                moveDto.ProductDescription = null;
            }
            else
            {
                if (GeneralOperateTaylorLift.Product.ToCharArray().All(Char.IsNumber))
                {
                    moveDto.ProductId = int.Parse(GeneralOperateTaylorLift?.Product);
                    moveDto.ProductDescription = GeneralOperateTaylorLift?.ProductDescription;
                }
                else if (GeneralOperateTaylorLift.Product.ToCharArray().Any(Char.IsLetter))
                    moveDto.ProductDescription = GeneralOperateTaylorLift?.Product;
            }
            var resultCode = _activityTypeRepository.FindByKey(ActivityCode.StartService);
            if (resultCode != null) moveDto.ActivityTypeId = resultCode.ActivityTypeId;

            if (GeneralOperateTaylorLift.CostCenter > 0)
            {
                moveDto.CostCenterId = GeneralOperateTaylorLift.CostCenter;
                moveDto.ChargeType = ChageTypeCostCenter;
            }
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.ShipmentID))
            {
                moveDto.ShipmentID = GeneralOperateTaylorLift?.ShipmentID;
                moveDto.ChargeType = ChageTypeShipment;
            }
            string[] formats = { DateFormats.TankDateFormat };
            if (GeneralOperateTaylorLift?.EquipmentTestDate25Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralOperateTaylorLift?.EquipmentTestDate25Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate25Year = dateCon;
            }
            if (GeneralOperateTaylorLift?.EquipmentTestDate5Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralOperateTaylorLift?.EquipmentTestDate5Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate5Year = dateCon;
            }
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.ChassisNumber)) moveDto.ChassisNumber = GeneralOperateTaylorLift?.ChassisNumber;
            if (!string.IsNullOrEmpty(GeneralOperateTaylorLift?.AuthorizationCode)) moveDto.AuthorizationCode = GeneralOperateTaylorLift?.AuthorizationCode;
            _shuttletServiceDto = moveDto;
        }
        public ICommand GoFinishOperateTaylorLiftCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection())
            {
                return;
            }

            if (!await VerifyWorkday()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogStartOperateTaylorLift)) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                _shuttletServiceDto.ServiceStartDate = DateTimeOffset.Now;
                _shuttletServiceDto.ServiceStartDateTZ = System.TimeZoneInfo.Local.ToString();
                SessionParameter.CurrentActivityChargeNo = LblNumber;

                var responseMove = await RegisterOperateTaylorLift();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                        var toUpdate = _operateTaylorLiftRepository.FindByKey(GeneralOperateTaylorLift.InternalId);
                        toUpdate.CurrentState = MoveState.StartedMove;
                        toUpdate.ServiceStartDate = _shuttletServiceDto.ServiceStartDate.Value.ToString("O");
                        _operateTaylorLiftRepository.Update(toUpdate);                                                  
                        await _navigator.PushAsync<FinishOperateTaylorLiftViewModel>(m =>
                        {
                            m.GeneralOperateTaylorLift = toUpdate;
                            m.GeneralShuttletServiceDTO = _shuttletServiceDto;
                        });
                        PersistPendentMove(State.Taylor(GeneralOperateTaylorLift.InternalId,_shuttletServiceDto));
                        _navigator.ClearNavigationStackToRoot();
                        break;
                }
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
            var request = new RegisterLogTaskDefinition { ActivityCode = activityCode };
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }
        private Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterOperateTaylorLift()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.StartTaylorLift, dateTimeTemp);

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
            _shuttletServiceDto.ActivityTypeId =shuttleTEventLogDTOTemp.ActivityTypeId??0;
            _shuttletServiceDto.PayBy = _sessionRepository.GetSessionObject().TypeUser;
            var request = new RegisterMoveTaskDefinition
            {
                GeneralOperateTaylorLift = GeneralOperateTaylorLift,
                ActivityCode = ActivityCode.StartTaylorLift,
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
                _operateTaylorLiftRepository.Delete(GeneralOperateTaylorLift.InternalId);
                await _navigator.PopToRootAsync();
            }
        });

        #endregion

        #region Variables

        private BEOperateTaylorLift _generalOperateTaylorLift;
        public BEOperateTaylorLift GeneralOperateTaylorLift
        {
            get { return _generalOperateTaylorLift; }
            set { SetProperty(ref _generalOperateTaylorLift, value); }
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
        private string _txtBlock;
        public string TxtBlock
        {
            get { return _txtBlock; }
            set
            {
                SetProperty(ref _txtBlock, value);
            }
        }

        private string _txtH34;
        public string TxtH34
        {
            get { return _txtH34; }
            set
            {
                SetProperty(ref _txtH34, value);
            }
        }

        private string _txtServiceType;
        public string TxtServiceType
        {
            get { return _txtServiceType; }
            set
            {
                SetProperty(ref _txtServiceType, value);
            }
        }

        private string _lblEquipmentType;
        public string LblEquipmentType
        {
            get { return _lblEquipmentType; }
            set
            {
                SetProperty(ref _lblEquipmentType, value);
            }
        }

        private string _txtEquipmentNumber;
        public string TxtEquipmentNumber
        {
            get { return _txtEquipmentNumber; }
            set
            {
                SetProperty(ref _txtEquipmentNumber, value);
            }
        }

        private string _txtEquipmentSize;
        public string TxtEquipmentSize
        {
            get { return _txtEquipmentSize; }
            set
            {
                SetProperty(ref _txtEquipmentSize, value);
            }
        }
        private string _txtEquipmentStatus;
        public string TxtEquipmentStatus
        {
            get { return _txtEquipmentStatus; }
            set
            {
                SetProperty(ref _txtEquipmentStatus, value);
            }
        }

        private string _txtChassisNumber;
        public string TxtChassisNumber
        {
            get { return _txtChassisNumber; }
            set { SetProperty(ref _txtChassisNumber, value); }
        }

        private string _txtProduct;
        public string TxtProduct
        {
            get { return _txtProduct; }
            set
            {
                SetProperty(ref _txtProduct, value);
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

        #endregion
    }
}
