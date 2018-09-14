using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Application.DTOs.Responses.Move;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Helpers;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class StartAdditionalServiceViewModel : StartViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IServiceRepository _serviceRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;

        private ShuttletServiceDTO _shuttletServiceDto;

        private static readonly string NotRequired = AppString.lblMsgNotRequired;
        private const string ChageTypeShipment = "S";
        private const string ChageTypeCostCenter = "C";
        private const int TimeDelay = 1000 * 1 * 1;
        private Timer _timer;

        public StartAdditionalServiceViewModel(INavigator navigator,
            IServiceRepository serviceRepository,
            IActivityTypeRepository activityTypeRepository,
            ISessionRepository sessionRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _serviceRepository = serviceRepository;
            _activityTypeRepository = activityTypeRepository;
            _sessionRepository = sessionRepository;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            if(_timer==null)
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
        public override void OnPushed()
        {
            base.OnPushed();
            SaveMoveDto();
            ShowResult();
        }
        private void TimerTaskCallback()
        {
            LblTime = $"{DateTime.Now:HH:mm:ss}";
            OnPropertyChanged("LblTime");
        }
        private void ShowResult()
        {
            if (!string.IsNullOrEmpty(GeneralService.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralService.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralService?.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralService.CostCenterName;
            }

            TxtBlock = string.IsNullOrEmpty(GeneralService.StartName) ? NotRequired : GeneralService.StartName;
            TxtH34 = GeneralService.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
            TxtServiceType = string.IsNullOrEmpty(GeneralService.MoveTypeDesc) ? NotRequired : GeneralService.MoveTypeDesc;
            LblEquipmentType = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? AppString.lblEquipmentType : GeneralService.EquipmentTypeDesc;
            TxtEquipmentNumber = string.IsNullOrEmpty(GeneralService.EquipmentNumber) ? NotRequired : GeneralService.EquipmentNumber;
            TxtEquipmentSize = string.IsNullOrEmpty(GeneralService.EquipmentSizeDesc) ? NotRequired : GeneralService.EquipmentSizeDesc;
            TxtEquipmentStatus = string.IsNullOrEmpty(GeneralService.EquipmentStatusDesc) ? NotRequired : GeneralService.EquipmentStatusDesc;
            TxtChassisNumber = string.IsNullOrEmpty(GeneralService.ChassisNumber) ? NotRequired : GeneralService.ChassisNumber;
            TxtProduct = string.IsNullOrEmpty(GeneralService.ProductDescription) ? NotRequired : GeneralService.ProductDescription;
            
            //OnPropertyChanged("LblId");
            //OnPropertyChanged("TxtBlock");
            //OnPropertyChanged("TxtH34");
            //OnPropertyChanged("TxtServiceType");
            //OnPropertyChanged("LblEquipmentType");
            //OnPropertyChanged("TxtEquipmentNumber");
            //OnPropertyChanged("TxtEquipmentSize");
            //OnPropertyChanged("TxtEquipmentStatus");
            //OnPropertyChanged("ChargeNo");
            //OnPropertyChanged("LblNumber");
        }
        private void SaveMoveDto()
        {
            if (GeneralService == null) return;
            var moveDto = new ShuttletServiceDTO()
            {
                ServiceOrderNo = GeneralService.InternalId,
                EquipmentNumber = GeneralService?.EquipmentNumber,
                ServiceId = GeneralService?.Service ?? 0,
                EquipmentTypeId = GeneralService?.EquipmentType ?? 0,
                EquipmentStatusId = GeneralService?.EquipmentStatus ?? 0,
                EquipmentSizeId = GeneralService?.EquipmentSize ?? 0,
                DispatchingPartyId = GeneralService?.DispatchingParty ?? 0,
                OriginLocationId = string.IsNullOrEmpty(GeneralService?.Start) ? 0 : int.Parse(GeneralService?.Start),
                HasH34 = GeneralService?.HasH34,
                DriverComments = GeneralService?.DriverComments
            };
            if (!string.IsNullOrEmpty(GeneralService?.Finish))
                moveDto.DestinationLocationId = int.Parse(GeneralService?.Finish);

            if (string.IsNullOrEmpty(GeneralService.Product))
            {
                moveDto.ProductId = null;
                moveDto.ProductDescription = null;
            }
            else
            {
                if (GeneralService.Product.ToCharArray().All(Char.IsNumber))
                {
                    moveDto.ProductId = int.Parse(GeneralService?.Product);
                    moveDto.ProductDescription = GeneralService?.ProductDescription;
                }
                else if (GeneralService.Product.ToCharArray().Any(Char.IsLetter))
                    moveDto.ProductDescription = GeneralService?.Product;
            }

            var resultCode = _activityTypeRepository.FindByKey(ActivityCode.StartService);
            if (resultCode != null) moveDto.ActivityTypeId = resultCode.ActivityTypeId;
            if (GeneralService.CostCenter > 0)
            {
                moveDto.CostCenterId = GeneralService.CostCenter;
                moveDto.ChargeType = ChageTypeCostCenter;
            }
            if (!string.IsNullOrEmpty(GeneralService?.ShipmentID))
            {
                moveDto.ShipmentID = GeneralService?.ShipmentID;
                moveDto.ChargeType = ChageTypeShipment;
            }
            string[] formats = { DateFormats.TankDateFormat };
            if (GeneralService?.EquipmentTestDate25Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralService?.EquipmentTestDate25Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate25Year = dateCon;
            }
            if (GeneralService?.EquipmentTestDate5Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralService?.EquipmentTestDate5Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate5Year = dateCon;
            }
            if (!string.IsNullOrEmpty(GeneralService?.ChassisNumber)) moveDto.ChassisNumber = GeneralService?.ChassisNumber;
            if (!string.IsNullOrEmpty(GeneralService?.Bobtail)) moveDto.AuthorizationCode = GeneralService?.Bobtail;
            _shuttletServiceDto = moveDto;
        }
        public ICommand GoFinishAdditionalServiceCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection())
            {                
                return;
            }

            if (!await VerifyWorkday()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogStartService)) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                _shuttletServiceDto.ServiceStartDate = DateTimeOffset.Now;
                _shuttletServiceDto.ServiceStartDateTZ = System.TimeZoneInfo.Local.ToString();
                SessionParameter.CurrentActivityChargeNo = LblNumber;

                var responseMove = await RegisterService();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                        var toUpdate = _serviceRepository.FindByKey(GeneralService.InternalId);
                        toUpdate.CurrentState = MoveState.StartedMove;
                        toUpdate.ServiceStartDate = _shuttletServiceDto.ServiceStartDate.Value.ToString("O");
                        _serviceRepository.Update(toUpdate);                       
                        await _navigator.PushAsync<FinishAdditionalServiceViewModel>(m =>
                        {
                            m.GeneralServiceBase = toUpdate;
                            m.GeneralShuttletServiceDTO = _shuttletServiceDto;
                        });
                        PersistPendentMove(State.Service(GeneralService.InternalId, _shuttletServiceDto));
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

        private Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterService()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.StartService, dateTimeTemp);

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
                GeneralService = GeneralService,
                ActivityCode = ActivityCode.StartService,
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
                _serviceRepository.Delete(GeneralService.InternalId);
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
        private Infrastructure.BackgroundTasks.TaskStatus RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition {ActivityCode = activityCode};
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            return Infrastructure.BackgroundTasks.TaskStatus.Pendent;
        }

        #region Variables
        private BEService _generalService;
        public BEService GeneralService
        {
            get { return _generalService; }
            set { SetProperty(ref _generalService, value); }
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