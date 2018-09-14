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
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Helpers.Timing;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.State;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public class StartDetentionViewModel : StartViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IDetentionRepository _detentionRepository;
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

        public StartDetentionViewModel(INavigator navigator,
            IDetentionRepository detentionRepository,
            IActivityTypeRepository activityTypeRepository,
            ISessionRepository sessionRepository,
            IWebServiceCallingInfomationProvider infomationProvider,
            INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _detentionRepository = detentionRepository;
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
        public override void OnPopped()
        {
            base.OnPopped();
        }

        #region Navigation
        private void TimerTaskCallback()
        {
            LblTime = $"{DateTime.Now:HH:mm:ss}";
            OnPropertyChanged("LblTime");
        }
        private void ShowResult()
        {
            if (!string.IsNullOrEmpty(GeneralDetention.ShipmentID))
            {
                LblId = AppString.lblShipmentId.ToUpper();
                LblNumber = GeneralDetention.ShipmentID;
            }
            else if (!string.IsNullOrEmpty(GeneralDetention?.CostCenterName))
            {
                LblId = AppString.lblCostCenter.ToUpper();
                LblNumber = GeneralDetention.CostCenterName;
            }

            TxtBlock = string.IsNullOrEmpty(GeneralDetention.StartName) ? NotRequired : GeneralDetention.StartName;
            TxtH34 = GeneralDetention.HasH34 ? AppString.lblH34Yes : AppString.lblH34No;
            TxtServiceType = string.IsNullOrEmpty(GeneralDetention.MoveTypeDesc) ? NotRequired : GeneralDetention.MoveTypeDesc;
            LblEquipmentType = string.IsNullOrEmpty(GeneralDetention.EquipmentNumber) ? AppString.lblEquipmentType : GeneralDetention.EquipmentTypeDesc;
            TxtEquipmentNumber = string.IsNullOrEmpty(GeneralDetention.EquipmentNumber) ? NotRequired : GeneralDetention.EquipmentNumber;
            TxtEquipmentSize = string.IsNullOrEmpty(GeneralDetention.EquipmentSizeDesc) ? NotRequired : GeneralDetention.EquipmentSizeDesc;
            TxtEquipmentStatus = string.IsNullOrEmpty(GeneralDetention.EquipmentStatusDesc) ? NotRequired : GeneralDetention.EquipmentStatusDesc;
            TxtChassisNumber = string.IsNullOrEmpty(GeneralDetention.ChassisNumber) ? NotRequired : GeneralDetention.ChassisNumber;
            TxtProduct = string.IsNullOrEmpty(GeneralDetention.ProductDescription) ? NotRequired : GeneralDetention.ProductDescription;

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
            if (GeneralDetention == null) return;

            var moveDto = new ShuttletServiceDTO()
            {
                ServiceOrderNo = GeneralDetention.InternalId,
                EquipmentNumber = GeneralDetention?.EquipmentNumber,
                ServiceId = GeneralDetention?.Service ?? 0,
                EquipmentTypeId = GeneralDetention?.EquipmentType ?? 0,
                EquipmentStatusId = GeneralDetention?.EquipmentStatus ?? 0,
                EquipmentSizeId = GeneralDetention?.EquipmentSize ?? 0,
                DispatchingPartyId = GeneralDetention?.DispatchingParty ?? 0,
                OriginLocationId = string.IsNullOrEmpty(GeneralDetention?.Start) ? 0 : int.Parse(GeneralDetention?.Start),
                HasH34 = GeneralDetention?.HasH34,
                DriverComments = GeneralDetention?.DriverComments
            };
            if (!string.IsNullOrEmpty(GeneralDetention?.Finish))
                moveDto.DestinationLocationId = int.Parse(GeneralDetention?.Finish);

            if (string.IsNullOrEmpty(GeneralDetention.Product))
            {
                moveDto.ProductId = null;
                moveDto.ProductDescription = null;
            }
            else
            {
                if (GeneralDetention.Product.ToCharArray().All(Char.IsNumber))
                {
                    moveDto.ProductId = int.Parse(GeneralDetention?.Product);
                    moveDto.ProductDescription = GeneralDetention?.ProductDescription;
                }
                else if (GeneralDetention.Product.ToCharArray().Any(Char.IsLetter))
                    moveDto.ProductDescription = GeneralDetention?.Product;
            }
            var resultCode = _activityTypeRepository.FindByKey(ActivityCode.StartService);
            if (resultCode != null) moveDto.ActivityTypeId = resultCode.ActivityTypeId;

            if (GeneralDetention.CostCenter > 0)
            {
                moveDto.CostCenterId = GeneralDetention.CostCenter;
                moveDto.ChargeType = ChageTypeCostCenter;
            }
            if (!string.IsNullOrEmpty(GeneralDetention?.ShipmentID))
            {
                moveDto.ShipmentID = GeneralDetention?.ShipmentID;
                moveDto.ChargeType = ChageTypeShipment;
            }
            string[] formats = { DateFormats.TankDateFormat };
            if (GeneralDetention?.EquipmentTestDate25Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralDetention?.EquipmentTestDate25Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate25Year = dateCon;
            }
            if (GeneralDetention?.EquipmentTestDate5Year?.Length > 0)
            {
                var dateCon = DateTime.ParseExact(GeneralDetention?.EquipmentTestDate5Year, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                moveDto.EquipmentTestDate5Year = dateCon;
            }
            if (!string.IsNullOrEmpty(GeneralDetention?.ChassisNumber)) moveDto.ChassisNumber = GeneralDetention?.ChassisNumber;
            if (!string.IsNullOrEmpty(GeneralDetention?.AuthorizationCode)) moveDto.AuthorizationCode = GeneralDetention?.AuthorizationCode;
            _shuttletServiceDto = moveDto;
        }
        public ICommand GoFinishDetentionCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection())
            {
                return;
            }

            if (!await VerifyWorkday()) return;
            if (!await ShowYesNoAlert(AppString.lblDialogStartDetention)) return;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                _shuttletServiceDto.ServiceStartDate = DateTimeOffset.Now;
                _shuttletServiceDto.ServiceStartDateTZ = System.TimeZoneInfo.Local.ToString();
                SessionParameter.CurrentActivityChargeNo = LblNumber;

                var responseMove = await RegisterDetention();
                switch (responseMove)
                {
                    case Infrastructure.BackgroundTasks.TaskStatus.Pendent:
                    case Infrastructure.BackgroundTasks.TaskStatus.Correct:
                        var toUpdate = _detentionRepository.FindByKey(GeneralDetention.InternalId);
                        toUpdate.CurrentState = MoveState.StartedMove;
                        toUpdate.ServiceStartDate = _shuttletServiceDto.ServiceStartDate.Value.ToString("O");
                        _detentionRepository.Update(toUpdate);
                        await _navigator.PushAsync<FinishDetentionViewModel>(m =>
                        {
                            m.GeneralDetention = toUpdate;
                            m.GeneralShuttletServiceDTO = _shuttletServiceDto;
                        });
                        PersistPendentMove(State.Detention(GeneralDetention.InternalId, _shuttletServiceDto));
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
        private Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterDetention()
        {
            DateTime dateTimeTemp = DateTime.Now;
            var shuttleTEventLogDTOTemp = _infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.StartDetention, dateTimeTemp);

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
                GeneralDetention = GeneralDetention,
                ActivityCode = ActivityCode.StartDetention,
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
                _detentionRepository.Delete(GeneralDetention.InternalId);
                await _navigator.PopToRootAsync();
            }
        });

        #endregion

        #region Variables

        private BEDetention _generalDetention;
        public BEDetention GeneralDetention
        {
            get { return _generalDetention; }
            set { SetProperty(ref _generalDetention, value); }
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