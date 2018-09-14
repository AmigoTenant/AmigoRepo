using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TSI.Xamarin.Forms.Logging.Abstract;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Requests.Security;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Application.DTOs.Responses.Security;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Security.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Navigation;
using XPO.ShuttleTracking.Mobile.Helpers;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.CustomException;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.ViewModel.Dialog;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Infrastructure.Settings;
using XPO.ShuttleTracking.Mobile.PubSubEvents;
using XPO.ShuttleTracking.Mobile.Services;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public sealed class LoginViewModel : TodayViewModel
    {
        private readonly INavigator _navigator;
        private readonly IPermissions _permissions;

        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMoveRepository _moveRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDetentionRepository _detentionRepository;
        private readonly IOperateTaylorLiftRepository _operateTaylorLiftRepository;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;
        private readonly INetworkInfoManager _networkInfoManager;

        private string _appVersion, _username, _password, _sourceEyeImg;
        private bool _isPassword, _showLanguageSelect;

        public LoginViewModel(INavigator navigator,
                            ISessionRepository sessionRepository,
                            IPermissions permissions,
                            IUserRepository userRepository,
                            IMoveRepository moveRepository,
                            IServiceRepository serviceRepository,
                            IDetentionRepository detentionRepository,
                            IOperateTaylorLiftRepository operateTaylorLiftRepository,
                            IConfigurationManager configurationManager,
                            IWebServiceCallingInfomationProvider infomationProvider,
                            INetworkInfoManager networkInfoManager)
        {
            _navigator = navigator;
            _permissions = permissions;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;

            _moveRepository = moveRepository;
            _serviceRepository = serviceRepository;
            _detentionRepository = detentionRepository;
            _operateTaylorLiftRepository = operateTaylorLiftRepository;

            Username = "";
            Password = "";

            AppVersion = string.Concat(configurationManager.GetString(ServiceSettings.Environment), " - ", AppString.lblLoginAppName, " ", configurationManager.GetString(ServiceSettings.SemanticVersion));
            SourceEyeImg = AppString.imgLoginEyeEnabled;
            IsPassword = false;

            LoginCommand = CreateCommand(OnLoginCommand);
            OnTapShowPassword = CreateCommand(TapShowPassword, canExecuteFunc: null);

            _infomationProvider = infomationProvider;
            _networkInfoManager = networkInfoManager;
        }
        
        public bool ShowLanguageSelect
        {
            get { return _showLanguageSelect; }
            set { SetProperty(ref _showLanguageSelect, value); }
        }
        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetProperty(ref _isPassword, value); }
        }

        public string SourceEyeImg
        {
            get { return _sourceEyeImg; }
            set { SetProperty(ref _sourceEyeImg, value); }
        }
        public string AppVersion
        {
            get { return _appVersion; }
            set { SetProperty(ref _appVersion, value); }
        }
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public ICommand LoginCommand { get; }
        public ICommand OnTapShowPassword { get; }
        private void TapShowPassword()
        {
            IsPassword = !IsPassword;
            SourceEyeImg = IsPassword ? AppString.imgLoginEyeDisabled : AppString.imgLoginEyeEnabled;
        }        
        private async Task OnLoginCommand()
        {

            if (!await CheckGpsConnection())
            {
                return;
            }            

            try
            {
                //Verify user input
                if (!await ValidateLoginInformation()) return;
                //Call the web service to authenticate
                var isLoggedIn = await VerifyCredentialsAsync();
                if (!isLoggedIn) return;
                
                await DownloadData();
                ForceLogGeneration();
                ClearHistory();

                var eventInfo = new Dictionary<string,string>();
                eventInfo.Add("DriverId", UserStatus.CurrentUser);
                Logger.Current.LogEvent(
                    SessionParameter.RecoveredFromCrash ? "ShuttleTracking has recovered from a crash" : "Log In",
                    eventInfo);

                MessagingCenter.Send(LoggedInMessage.Empty, LoggedInMessage.Name);
                
                await _navigator.PushAsync<MainMenuViewModel>(m => m.UserName = Username);

                //remove the go back
                _navigator.ClearNavigationStack();
            }
            catch (ConnectivityException)
            {
                await ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.ConnectiviyError);
            }
            catch (GpsEnableException)
            {
                await ShowOkAlert(AppString.lblAppName, AppString.lblGpsException);
            }
            catch (Exception e)
            {
                await ShowOkAlert(AppString.lblError, e.Message);
                //throw;
            }
        }

        private void ForceLogGeneration()
        {
            var varEnableLogGlobally = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EnableLogGlobally), out varEnableLogGlobally))
                varEnableLogGlobally = 0;
            if (varEnableLogGlobally == 0) return;

            var session = _sessionRepository.GetSessionObject();
            session.RegisterLog = varEnableLogGlobally == 1;
            _sessionRepository.Update(session);
            MessagingCenter.Send(new PersistLogMessage(session.RegisterLog), PersistLogMessage.Name);
        }

        private async void ClearHistory()
        {
            try
            {
                int keepInHistoryDays;
                var parameter =Parameters.Get(ParameterCode.KeepInHistoryDays);
                if(!int.TryParse(parameter, out keepInHistoryDays))keepInHistoryDays = 7;

                var historyDayClear = DateTime.Now.Subtract(TimeSpan.FromDays(keepInHistoryDays));

                var lstMoves = _moveRepository.GetAllBefore(historyDayClear).ToList();
                var lstTaylorLift = _operateTaylorLiftRepository.GetAllBefore(historyDayClear).ToList();
                var lstServices = _serviceRepository.GetAllBefore(historyDayClear).ToList();
                var lstDetentions = _detentionRepository.GetAllBefore(historyDayClear).ToList();

                var lenght = lstMoves.Count + lstServices.Count + lstDetentions.Count + lstTaylorLift.Count;
                if (lenght <= 0) return;

                Logger.Current.LogInfo($"Delete {lenght} record(s)");
                foreach (var act in lstMoves) _moveRepository.Delete(act.InternalId);
                foreach (var act in lstServices) _serviceRepository.Delete(act.InternalId);
                foreach (var act in lstDetentions) _detentionRepository.Delete(act.InternalId);
                foreach (var act in lstTaylorLift) _operateTaylorLiftRepository.Delete(act.InternalId);

                _operateTaylorLiftRepository.Flush();
            }
            catch (Exception ex)
            {
                Logger.Current.LogWarning($"Error deleting history - {ex}");
                await ShowOkAlert(AppString.lblError, AppString.errorActivityHistory);
            }
        }

        #region Login process
        private async void CheckIfUserHasAcceptedTerms()
        {
            if(_userRepository.FindAll(x => x.ReadTos).Any()) return;

            await _navigator.PushModalAsync<ConfirmTosViewModel>();
        }
        private async Task<bool> VerifyCredentialsAsync()
        {
            var result = false;
            using (StartLoading(AppString.lblLoginLoggingIn))
            {
                await Task.Run(async () =>
                {

                    try
                    {
                        var shuttleTEventLogDtoTemp =_infomationProvider.FillTaskShuttleTEventLogDTO(ActivityCode.Authentication,DateTime.Now);
                        var authorizationRequest = new AuthorizationRequest
                        {
                            IsAutoDateTime = shuttleTEventLogDtoTemp.IsAutoDateTime,
                            IsSpoofingGPS = shuttleTEventLogDtoTemp.IsSpoofingGPS,
                            IsRootedJailbreaked = shuttleTEventLogDtoTemp.IsRootedJailbreaked,
                            Platform = shuttleTEventLogDtoTemp.Platform,
                            OSVersion = shuttleTEventLogDtoTemp.OSVersion,
                            AppVersion = shuttleTEventLogDtoTemp.AppVersion,
                            Latitude = shuttleTEventLogDtoTemp.Latitude,
                            Longitude = shuttleTEventLogDtoTemp.Longitude,
                            Accuracy = shuttleTEventLogDtoTemp.Accuracy,
                            LocationProvider = shuttleTEventLogDtoTemp.LocationProvider,
                            ReportedActivityTimeZone = shuttleTEventLogDtoTemp.ReportedActivityTimeZone,
                            ReportedActivityDate = shuttleTEventLogDtoTemp.ReportedActivityDate,
                            ActivityTypeId = 1
                        };


                        var loginTask = new LoginTaskDefinition
                        {
                            User = Username,
                            Password = Password,
                            ActivityCode = ActivityCode.Authentication,
                            AuthorizationRequest = authorizationRequest,
                            Timezone = TimeZoneInfo.Local.ToString(),
                            UserIdOrGuid = _networkInfoManager.GetIdOrGuid()
                        };

                        var response = await TaskManager.Current.ExecuteTaskAsync<ResponseDTO<ShuttleTUserDTO>>(loginTask);
                        if (response != null && response.IsValid)
                        {
                            UserStatus.CurrentUser = Username;
                        }
                        else
                        {
                            var errorMessage = response?.Messages?.First()?.Message;
                            errorMessage = string.IsNullOrEmpty(errorMessage) ?
                            AppString.lblErrorUnknown : errorMessage;
                            await ShowOkAlert(AppString.lblError, errorMessage);
                        }
                        result = response != null && response.IsValid;
                    }
                    catch (IdentityException)
                    {
                        await ShowOkAlert(AppString.lblError, AppString.errorInvalidUsername);
                    }
                    catch (ConnectivityException)
                    {
                        await
                            ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.ConnectiviyError);
                    }
                    catch (GpsEnableException)
                    {
                        await ShowOkAlert(AppString.lblAppName, Infrastructure.Resources.StringResources.GSPError);
                    }
                    catch (InvalidOperationException ex)
                    {
                        await ShowOkAlert(AppString.lblAppName, ex.Message);
                    }
                    catch (Exception)
                    {
                        await
                            ShowError(ErrorCode.LoginVerifyCredentials,
                                Infrastructure.Resources.StringResources.GenericError);
                    }
                });
                return result;
            }
        }

        private async Task<bool> CheckIfUserHasAcceptedPermissions(Action forShow)
        {
            var isChecked = await SetPermissions(string.Empty, true);
            if (isChecked)
            {
                forShow();
                return true;
            }
            await _navigator.PushModalAsync<ConfirmPermissionViewModel>(x => x.ForShow = forShow);
            return false;
        }
        private async Task<bool> SetPermissions(string informationMessage,bool onlyCheck)
        {
            var o = new PermissionsBuild
            {                
                PermissionToTest = string.Empty,
                Reason = informationMessage,
                AsPopup = true
            };
            if (onlyCheck)
            {
                return _permissions.Has(o);
            }
            _permissions.RequestPermissionsReason(o);
            return await o.Result.Task;
        }
        private async Task<bool> ValidateLoginInformation()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                var message = string.Format(AppString.lblFormatFieldLengthMin, AppString.lblLoginUsername, 3);
                await MessageService.Current.ShowMessageAsync(AppString.titleLogin, message, AppString.btnDialogOk);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            { 
                var message = string.Format(AppString.lblFormatFieldLengthMin, AppString.lblLoginPassword, 5);
                await MessageService.Current.ShowMessageAsync(AppString.titleLogin, message, AppString.btnDialogOk);
                return false;
            }

            return true;
        }
        #endregion        
        private async Task DownloadData()
        {
            ResponseDTO resp = null;

            using (StartLoading(AppString.lblDialogUpdatingMaster))
            {                
                await Task.Run(async () =>
                {
                    var task = new MasterDataDownloaderTask();
                    resp = await TaskManager.Current.ExecuteTaskAsync<ResponseDTO>(task);
                });                
            }

            if (!resp.IsValid)
            {
                var confirm = await MessageService.Current.ConfirmAsync(AppString.lblMasterData, AppString.errorMasterData,AppString.lblRetry,AppString.lblContinue);
                if (confirm) await DownloadData();
            }
        }
        public override async void OnPushed()
        {
            base.OnPushed();
            ShowLanguageSelect = false;

            var session = _sessionRepository.GetSessionObject();
            Username = session.Username ?? string.Empty;

            await CheckIfUserHasAcceptedPermissions(CheckIfUserHasAcceptedTerms);                        
        }
    }
}