using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using TSI.Xamarin.Forms.Persistence.Storage;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Entity.Move;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Entity;
using XPO.ShuttleTracking.Mobile.Entity.Service;
using XPO.ShuttleTracking.Mobile.Entity.Detention;
using XPO.ShuttleTracking.Mobile.Entity.OperateTaylorLift;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.PubSubEvents;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.PubSubEvents;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public sealed class MainMenuViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IPersistentStorageManager _persistentStorageManager;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;


        private BEMove _generalMove;
        private int _selectedTab;
        private string _textOnDuty;
        private string _textOnDutyMsg;
        private string _userName;
        private bool _showViewSummary;
        private bool _showArriveFacility;
        private bool _showDepartFacility;
        private bool _isOnBreak;
        private bool _showArrivedHeader;
        private bool _showOnDuty;
        private bool _showDailyActivities;
        private string _textOnDutyTime;
        private string _textStartedDayTime;
        private string _textStartedDayMsg;
        private Color _arrivedOnOffDutyColor;
        private bool _isWorking;
        private bool OffBreakByDefault;
        private bool _isPerHour;
        private string username;


        public MainMenuViewModel(INavigator navigator,
                                IMoveRepository moveRepository,
                                IServiceRepository serviceRepository,
                                IDetentionRepository detentionRepository,
                                IOperateTaylorLiftRepository operateTaylorLiftRepository,
                                ISessionRepository sessionRepository,
                                IPersistentStorageManager persistentStorageManager,
                                IWebServiceCallingInfomationProvider infomationProvider,
                                INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;
            _sessionRepository = sessionRepository;
            _persistentStorageManager = persistentStorageManager;
            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;

            //Commands
            this.OnTapSummary = CreateCommand(ShowSummaryScreen);
            this.PropertyChanged += OnDutyChanged;
        }

        private async Task ShowSummaryScreen()
        {
            if (_isPerHour)
                await _navigator.PushAsync<SummaryPerHourViewModel>();
            else
                await _navigator.PushAsync<SummaryViewModel>();
        }

        private void UpdateInternetStatus(bool state)
        {
            SessionParameter.IsConnectivity = state;
            var connType = CrossConnectivity.Current.ConnectionTypes.Any()
                ? CrossConnectivity.Current.ConnectionTypes.First()
                : ConnectionType.Other;
            SessionParameter.LocationProvider = connType == ConnectionType.Cellular ? ConnectionTypeName.gps : ConnectionTypeName.network;
        }

        public override void OnPushed()
        {
            UpdateInternetStatus(CrossConnectivity.Current.IsConnected);
            Title = AppString.titleMainMenu;
            TextOnDuty = AppString.lblMainMenuOnDuty;
            base.OnPushed();

            var parameter = Parameters.Get(ParameterCode.OffBreakByDefault);
            if (!bool.TryParse(parameter, out OffBreakByDefault))
                OffBreakByDefault = false;

            var session = _sessionRepository.GetSessionObject();
            username = session.Username;
            _isOnBreak = session.IsOnBreak;
            IsOnBreak = session.IsOnBreak;
            _isPerHour = session.TypeUser == UserTypeCode.PerHour;
            ArrivedOnOffDutyColor = ReturnColorFromResources("ColorGeoFencing");
            OnPropertyChanged("IsOnBreak");
            UpdateVisibleViews(session);
        }

        public override void OnAppearing()
        {
            Logger.Current.LogInfo($"Now Showing: {this.GetType().FullName}");
            MessagingCenter.Subscribe<GpsExceptionMessage>(this, GpsExceptionMessage.Name, ShowGpsError);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<GpsExceptionMessage>(this, GpsExceptionMessage.Name);
        }

        public Color ReturnColorFromResources(string resource)
        {
            if (Xamarin.Forms.Application.Current != null)
                return (Color)Xamarin.Forms.Application.Current.Resources[resource];
            return Color.Transparent;
        }

        private async void ShowGpsError(GpsExceptionMessage gpsExceptionMessage)
        {
            await ShowOkAlert(AppString.lblAppName, AppString.lblGpsException);
        }
        public bool IsWorking
        {
            get { return _isWorking; }
            private set { SetProperty(ref _isWorking, value); }
        }

        private bool _isWorkingPerMove;
        public bool IsWorkingPerMove
        {
            get { return _isWorkingPerMove; }
            private set { SetProperty(ref _isWorkingPerMove, value); }
        }

        void UpdateVisibleViews(BESession status)
        {
            //Shows
            IsWorking = status.HasArrived && !status.IsOnBreak;
            IsWorkingPerMove = IsWorking && status.TypeUser.Equals(UserTypeCode.PerMove);
            ShowOnDuty = true;
            ShowViewSummary = true;
            ShowArriveFacility = !status.HasArrived;
            ShowDepartFacility = status.HasArrived;
            ShowArrivedHeader = status.HasArrived;
            ShowDailyActivities = true;

            //Texts
            TextOnDuty = string.Format(
                status.IsOnBreak
                    ? (status.HasArrived ? AppString.lblMainMenuOffDutyMsg : AppString.lblMainMenuOffDuty)
                    : AppString.lblMainMenuOnDuty, "");
            OnPropertyChanged("TextOnDuty");
            TextOnDutyMsg = status.IsOnBreak
                    ? (status.HasArrived ? status.LastBreakInfo : "") : "";
            TextOnDutyTime = status.LastBreakInfo;
            TextStartedDayMsg = AppString.lblMainMenuStartedDay;
            TextStartedDayTime = String.IsNullOrEmpty(status.LastDateArrivalInfo) ? DateTime.Now.ToString(DateFormats.CurrentWorkday) : DateTime.ParseExact(status.LastDateArrivalInfo, "O", CultureInfo.InvariantCulture).ToString(DateFormats.WorkDayTime);

            //Colors
            ArrivedOnOffDutyColor = status.IsOnBreak
                ? ReturnColorFromResources("ColorOnBreak")
                : ReturnColorFromResources("ColorOnDuty");
        }

        #region Commands
        public ICommand LogOutCommand => CreateCommand(async () =>
        {
            if (!await ShowYesNoAlert(AppString.lblDialogLogOutTitle, AppString.lblDialogLogOutMessage)) return;

            var response = await RegisterLogOut();
            _persistentStorageManager.RemoveValue(SecuritySettings.TokenKey);
            _persistentStorageManager.RemoveValue(SecuritySettings.RefreshTokenKey);
            _persistentStorageManager.RemoveValue(SecuritySettings.ExpirationKey);

            await _navigator.PushAsync<LoginViewModel>();
            _navigator.ClearNavigationStack();
            MessagingCenter.Send(LoggedOutMessage.Empty, LoggedOutMessage.Name);
        }
        );

        public ICommand OnArriveFacilityCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            var result = await ConfirmAction(AppString.lblMainMenuConfirmStartWorkday);
            if (!result) return;

            var session = _sessionRepository.GetSessionObject();
            session.HasArrived = true;
            session.LastDateArrivalInfo = DateTime.Now.ToString("O");
            session.TypeUserHasChanged = false;
            if (OffBreakByDefault)
                IsOnBreak = session.IsOnBreak = false;

            using (StartLoading(AppString.lblGeneralLoading))
            {
                RegisterLog(ActivityCode.StartWorkday);
                RegisterLog(session.IsOnBreak ? ActivityCode.OnBreak : ActivityCode.OffBreak);
                UpdateVisibleViews(session);
                _sessionRepository.Update(session);
            }
        });

        public ICommand OnDepartFacilityCommand => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;
            if (!await ConfirmAction(AppString.lblMainMenuConfirmFinishWorkday)) return;

            var session = _sessionRepository.GetSessionObject();
            session.HasArrived = false;
            ShowArriveFacility = !session.HasArrived;
            ShowDepartFacility = session.HasArrived;
            using (StartLoading(AppString.lblGeneralLoading))
            {
                RegisterLog(ActivityCode.FinishWorkday);
                _sessionRepository.Update(session);
                UpdateVisibleViews(session);
            }
        });
        private async Task<bool> VerifyWorkday()
        {
            var session = _sessionRepository.GetSessionObject();
            //Si no hay fechas previas, salir
            if (String.IsNullOrEmpty(session.LastDateArrivalInfo)) return true;
            DateTime lastStoredWorkday = DateTime.ParseExact(session.LastDateArrivalInfo, "O", CultureInfo.InvariantCulture); //Leer ultimo workday almacenada

            if (DateTime.Now.Date.CompareTo(lastStoredWorkday.Date) <= 0) return true;
            await ShowOkAlert(AppString.lblTooltipFinishPreviousWorkday);
            return false;
        }

        private async Task<bool> VerifyUserTypeUpdate()
        {
            var session = _sessionRepository.GetSessionObject();
            if (session.TypeUserHasChanged)
            {
                var currentWorkdayDate = string.IsNullOrEmpty(session.LastDateArrivalInfo)
                    ? DateTime.Now.Date
                    : DateTimeParse(session.LastDateArrivalInfo)?.Date;
                if (DidDriverRegisterAnActivity(currentWorkdayDate.Value.Date))
                {
                    await
                        ShowOkAlert(AppString.lblTooltipTitleSettingsUpdated,
                            string.Format(AppString.lblTooltipUserTypeHasChanged,
                                session.TypeUser == UserTypeCode.PerHour ? AppString.lblPerMove : AppString.lblPerHour,
                                session.TypeUser == UserTypeCode.PerHour ? AppString.lblPerHour : AppString.lblPerMove));
                    return false;
                }
                else
                {
                    session.TypeUserHasChanged = false;
                    _sessionRepository.Update(session);
                }
            }
            return true;
        }

        private bool DidDriverRegisterAnActivity(DateTime date)
        {
            if (_moveRepository.FindAll(x => x.CreationDate.Date >= date).Any() ||
                _serviceRepository.FindAll(x => x.CreationDate.Date >= date).Any() ||
                _detentionRepository.FindAll(x => x.CreationDate.Date >= date).Any() ||
                _operateTaylorLiftRepository.FindAll(x => x.CreationDate.Date >= date).Any())
            {
                return true;
            }
            return false;
        }
        private DateTime? DateTimeParse(string date)
        {
            if (date == null) return null;
            try
            {
                return DateTime.ParseExact(date, "O", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                ShowError(ErrorCode.MainMenuDateTimeParse, AppString.lblErrorReadingDateTime);
            }
            return null;
        }
        public ICommand OnTapRegisterMoves => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (await VerifyWorkday())
                if (await VerifyUserTypeUpdate())
                    await GetMove(ViewOptions.RegisterMove);
        });

        public ICommand OnTapAdditionalService => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (await VerifyWorkday())
                if (await VerifyUserTypeUpdate())
                    await GetService();
        });

        public ICommand OnTapAddDetention => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (await VerifyWorkday())
                if (await VerifyUserTypeUpdate())
                    await GetDetention();
        });
        public ICommand OnTapAddOperateTaylorLift => CreateCommand(async () =>
        {
            if (!await CheckGpsConnection()) return;

            if (await VerifyWorkday())
                if (await VerifyUserTypeUpdate())
                    await GetOperateTaylorLift();
        });

        public ICommand OnTapSummary { get; }

        public ICommand SettingsCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<SettingsViewModel>();
        });
        public ICommand TosCommand => CreateCommand(async () =>
        {
            await _navigator.PushAsync<TermOfServicesViewModel>();
        });

        public ICommand OnTapDailyActivities => CreateCommand(async () => await _navigator.PushAsync<DailyActivitiesFilteredViewModel>());
        public ICommand OnTapStoreAndForward => CreateCommand(async () => await _navigator.PushAsync<StoreForwardViewModel>());
        public ICommand OnTapAcknowledgeMove => CreateCommand(async () => await _navigator.PushAsync<AcknowledgeMoveViewModel>(x => x.SelectedChargeNumbers = null));
        #endregion

        private void RegisterLog(string activityCode)
        {
            var request = new RegisterLogTaskDefinition();
            _infomationProvider.FillTaskDefinition(ref request);
            request.ActivityCode = activityCode;
            TaskManager.Current.RegisterStoreAndForward(request);
        }

        private Task<Infrastructure.BackgroundTasks.TaskStatus> RegisterLogOut()
        {
            var request = new RegisterLogTaskDefinition();
            _infomationProvider.FillTaskDefinition(ref request);
            request.ActivityCode = ActivityCode.Logout;
            return TaskManager.Current.TryExecuteOnline<ResponseDTO>(request);
        }

        private async Task<bool> ConfirmAction(string msg)
        {
            return await ShowYesNoAlert(msg);
        }

        private async Task GetMove(ViewOptions _viewOptions)
        {
            var lstMove = _moveRepository.FindAll(x => (x.CurrentState == MoveState.SavedMove) && x.IsModified);
            var currentMove = lstMove.FirstOrDefault();
            var responseMove = new BEMove();
            var needTocleanObject = false;
            if (currentMove == null)
                needTocleanObject = true;
            else
            {
                var selection = await ShowYesNoAlert(AppString.lblDialogMove);
                if (selection)
                    responseMove = currentMove;
                else
                {
                    needTocleanObject = true;
                    _moveRepository.Delete(currentMove.InternalId);
                }

            }
            if (needTocleanObject)
            {
                responseMove.InternalId = Guid.NewGuid();
                responseMove.CurrentState = MoveState.CreatedMove;
                responseMove.IsModified = true;
                responseMove.UserName = username;
                _moveRepository.Add(responseMove);
            }
            switch (_viewOptions)
            {
                case ViewOptions.RegisterMove:
                    await _navigator.PushAsync<RegisterMoveViewModel>(x =>
                    {
                        x.GeneralMove = responseMove;
                        x.isAnotherMove = false;
                    });
                    break;
            }
        }

        private async Task GetService()
        {
            var lstService = _serviceRepository.FindAll(x => (x.CurrentState == MoveState.SavedMove) && x.IsModified);
            var currentService = lstService.FirstOrDefault();
            var responseService = new BEService();
            var needTocleanObject = false;
            if (currentService == null)
                needTocleanObject = true;
            else
            {
                var selection = await ShowYesNoAlert(AppString.lblDialogService);
                if (selection)
                {
                    responseService = currentService;
                }
                else
                {
                    needTocleanObject = true;
                    _serviceRepository.Delete(currentService.InternalId);
                }

            }
            if (needTocleanObject)
            {
                responseService.InternalId = Guid.NewGuid();
                responseService.CurrentState = MoveState.CreatedMove;
                responseService.IsModified = true;
                responseService.UserName = username;
                _serviceRepository.Add(responseService);
            }

            await _navigator.PushAsync<RegisterAdditionalServiceViewModel>(x =>
            {
                x.GeneralService = responseService;
                x.isAnotherService = false;
            });
        }

        private async Task GetDetention()
        {
            var lstDetention = _detentionRepository.FindAll(x => (x.CurrentState == MoveState.SavedMove) && x.IsModified);
            var currentDetention = lstDetention.FirstOrDefault();
            var responseDetention = new BEDetention();
            var needTocleanObject = false;
            if (currentDetention == null)
                needTocleanObject = true;
            else
            {
                var selection = await ShowYesNoAlert(AppString.lblDialogDetention);
                if (selection)
                {
                    responseDetention = currentDetention;
                }
                else
                {
                    needTocleanObject = true;
                    _detentionRepository.Delete(currentDetention.InternalId);
                }

            }
            if (needTocleanObject)
            {
                responseDetention.InternalId = Guid.NewGuid();
                responseDetention.CurrentState = MoveState.CreatedMove;
                responseDetention.IsModified = true;
                responseDetention.UserName = username;
                _detentionRepository.Add(responseDetention);
            }
            await _navigator.PushAsync<RegisterDetentionViewModel>(x =>
            {
                x.GeneralDetention = responseDetention;
                x.isAnotherDetention = false;
            });
        }

        private async Task GetOperateTaylorLift()
        {
            var lstOperateTaylorLift = _operateTaylorLiftRepository.FindAll(x => (x.CurrentState == MoveState.SavedMove) && x.IsModified);
            var currentOperateTaylorLift = lstOperateTaylorLift.FirstOrDefault();
            var responseOperateTaylorLift = new BEOperateTaylorLift();
            var needTocleanObject = false;
            if (currentOperateTaylorLift == null)
                needTocleanObject = true;
            else
            {
                var selection = await ShowYesNoAlert(AppString.lblDialogOperateTaylorLift);
                if (selection)
                {
                    responseOperateTaylorLift = currentOperateTaylorLift;
                }
                else
                {
                    needTocleanObject = true;
                    _operateTaylorLiftRepository.Delete(currentOperateTaylorLift.InternalId);
                }

            }
            if (needTocleanObject)
            {
                responseOperateTaylorLift.InternalId = Guid.NewGuid();
                responseOperateTaylorLift.CurrentState = MoveState.CreatedMove;
                responseOperateTaylorLift.IsModified = true;
                responseOperateTaylorLift.UserName = username;
                _operateTaylorLiftRepository.Add(responseOperateTaylorLift);
            }
            await _navigator.PushAsync<RegisterOperateTaylorLiftViewModel>(x =>
            {
                x.GeneralOperateTaylorLift = responseOperateTaylorLift;
                x.isAnotherOperateTaylorLift = false;
            });
        }
        public string TextOnDutyMsg
        {
            get { return _textOnDutyMsg; }
            set { SetProperty(ref _textOnDutyMsg, value); }
        }
        public string TextOnDuty
        {
            get { return _textOnDuty; }
            set { SetProperty(ref _textOnDuty, value); }
        }

        public string TextOnDutyTime
        {
            get { return _textOnDutyTime; }
            set { SetProperty(ref _textOnDutyTime, value); }
        }

        public string TextStartedDayMsg
        {
            get { return _textStartedDayMsg; }
            set { SetProperty(ref _textStartedDayMsg, value); }
        }
        public string TextStartedDayTime
        {
            get { return _textStartedDayTime; }
            set { SetProperty(ref _textStartedDayTime, value); }
        }

        public Color ArrivedOnOffDutyColor
        {
            get { return _arrivedOnOffDutyColor; }
            set { SetProperty(ref _arrivedOnOffDutyColor, value); }
        }

        public bool IsOnBreak
        {
            get { return _isOnBreak; }
            set
            {
                if (_isOnBreak == value) return;
                SetProperty(ref _isOnBreak, value);
            }
        }

        void UpdateAppStatus(bool value)
        {
            var session = _sessionRepository.GetSessionObject();
            session.IsOnBreak = value;
            session.LastBreakInfo = DateTime.Now.ToString(DateFormats.TimeHHmm);
            if (session.HasArrived)
            {
                RegisterLog(session.IsOnBreak ? ActivityCode.OnBreak : ActivityCode.OffBreak);
            }
            UpdateVisibleViews(session);
            _sessionRepository.Update(session);
        }

        private void OnDutyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(IsOnBreak))
            {
                Task.Run(() => UpdateAppStatus(IsOnBreak));
            }
        }

        #region Shows Controls
        public bool ShowArrivedHeader
        {
            get { return _showArrivedHeader; }
            set { SetProperty(ref _showArrivedHeader, value); }
        }
        public bool ShowOnDuty
        {
            get { return _showOnDuty; }
            set { SetProperty(ref _showOnDuty, value); }
        }
        public bool ShowViewSummary
        {
            get { return _showViewSummary; }
            set { SetProperty(ref _showViewSummary, value); }
        }
        public bool ShowArriveFacility
        {
            get { return _showArriveFacility; }
            set { SetProperty(ref _showArriveFacility, value); }
        }
        public bool ShowDepartFacility
        {
            get { return _showDepartFacility; }
            set { SetProperty(ref _showDepartFacility, value); }
        }
        public bool ShowDailyActivities
        {
            get { return _showDailyActivities; }
            set { SetProperty(ref _showDailyActivities, value); }
        }
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public enum ViewOptions
        {
            RegisterMove,
            AdditionalService,
            Detention,
            OperateTaylorLift
        }
        #endregion
    }
}