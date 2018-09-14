using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PCLStorage;
using XPO.ShuttleTracking.Mobile.Infrastructure.Constants;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Settings;
using XPO.ShuttleTracking.Mobile.Model;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using TSI.Xamarin.Forms.Logging.Abstract;
using TSI.Xamarin.Forms.Logging.Share;
using TSI.Xamarin.Forms.Logging.Zip.Abstract;
using TSI.Xamarin.Forms.Mvvm.Views;
using TSI.Xamarin.Forms.Persistence.Storage;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Responses.Common;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Services;

namespace XPO.ShuttleTracking.Mobile.ViewModel
{
    public sealed class SettingsViewModel : TodayViewModel
    {
        private readonly ILogger _logger;        
        private readonly IShareManager _shareManager;
        private readonly IZipper _zipper;
        private readonly ISessionRepository _sessionRepository;
        private readonly IConfigurationManager _configurationManager;
        private readonly IFolderProvider _folderProvider;
        private readonly IPersistentStorageManager _persistentStorageManager;

        public SettingsViewModel(IZipper zipper,
            IShareManager shareManager,            
            ISessionRepository sessionRepository,
            IFolderProvider folderProvider,
            IPersistentStorageManager persistentStorageManager,
            IConfigurationManager configurationManager)
        {
            _logger = Logger.Current;
            _zipper = zipper;            
            _shareManager = shareManager;
            _sessionRepository = sessionRepository;
            _configurationManager = configurationManager;
            _folderProvider = folderProvider;
            _persistentStorageManager = persistentStorageManager;

            RefreshMasterDataCommand = CreateCommand(RefreshMasterData);
        }

        public ICommand RefreshMasterDataCommand { get; private set; }

        private async Task RefreshMasterData()
        {
            ResponseDTO resp = null;
            using (StartLoading(AppString.lblDialogUpdatingMaster))
            {
                _logger.LogInfo("Updating Master Data ...");
                await Task.Run(async () =>
                {
                    var task = new MasterDataDownloaderTask {NeedToClean = true};
                    resp = await TaskManager.Current.ExecuteTaskAsync<ResponseDTO>(task);
                    var masterDataDate = _persistentStorageManager.ReadValue<string>(AppSettings.MasterDataLastUpdate);
                    if (!string.IsNullOrEmpty(masterDataDate)) MasterDataLastUpdate = masterDataDate;
                });

                if (!resp.IsValid)
                {
                    var confirm = await MessageService.Current.ConfirmAsync(AppString.lblMasterData, AppString.errorMasterData, AppString.lblRetry, AppString.lblContinue);
                    if (confirm) await RefreshMasterData();
                }
                _logger.LogInfo("Master data Update complete!");
                ForceLogGeneration();
            }
        }

        public override void OnPushed()
        {
            base.OnPushed();
            LoadData();
        }

        private void LoadData()
        {
            AppVersion = _configurationManager.GetString(ServiceSettings.SemanticVersion);

            var session = _sessionRepository.GetSessionObject();
            DriverID = session.Username;
            IsLogEnabled = session.RegisterLog;

            var keepInHistoryDays = 7;
            var parameter = Parameters.Get(ParameterCode.KeepInHistoryDays);
            if (!int.TryParse(parameter, out keepInHistoryDays))
                keepInHistoryDays = 7;

            DeleteHistory = string.Concat(keepInHistoryDays, " ", keepInHistoryDays == 1 ? AppString.lblDay : AppString.lblDays);

            if (session.TypeUser == UserTypeCode.PerHour)
                TypeUser = AppString.lblPerHour;
            else if (session.TypeUser == UserTypeCode.PerMove)
                TypeUser = AppString.lblPerMove;
            else
                TypeUser = string.Empty;
            //MasterData label date
            var masterDataDate = _persistentStorageManager.ReadValue<string>(AppSettings.MasterDataLastUpdate);
            if (!string.IsNullOrEmpty(masterDataDate)) MasterDataLastUpdate = masterDataDate;
        }

        public ICommand SendLogCommand => CreateCommand(async() =>
        {
            //Send 
            using (StartLoading(AppString.lblSettingsCreatingLogFile))
            {
                _logger.LogInfo("Send log command activated");
                using (StartLoading("Creating diagnostic files..."))
                {
                    var rootFolder = await FileSystem.Current.GetFolderFromPathAsync(_folderProvider.GetFolderForShare());
                    var logsFolder = await rootFolder.CreateFolderAsync(ShuttleLogger.LogsFolderName, CreationCollisionOption.OpenIfExists);
                    var zipFolder = await rootFolder.CreateFolderAsync(ShuttleLogger.ZippedLogsFolderName, CreationCollisionOption.OpenIfExists);
                    var zipFile = await zipFolder.CreateFileAsync($"Log {DateTime.Now:yyyyMMdd_HHmm}.zip", CreationCollisionOption.ReplaceExisting);

                    ShuttleLogger.LogWriteMutex.WaitOne();
                    zipFile = await _zipper.CreateZipFromFilesInFolderAsync(zipFile, logsFolder);
                    ShuttleLogger.LogWriteMutex.ReleaseMutex();
                    if (zipFile != null)
                    {
                        _shareManager.ShareFile(zipFile);
                    }
                }
            }
        });

        private string _masterDataLastUpdate;
        public string MasterDataLastUpdate
        {
            get { return _masterDataLastUpdate; }
            protected set { SetProperty(ref _masterDataLastUpdate, value); }
        }
        private string _driverID;
        public string DriverID
        {
            get { return _driverID; }
            protected set { SetProperty(ref _driverID, value); }
        }
        private string _typeUser;
        public string TypeUser
        {
            get { return _typeUser; }
            protected set { SetProperty(ref _typeUser, value); }
        }
        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            protected set { SetProperty(ref _appVersion, value); }
        }

        private string _deleteHistory;
        public string DeleteHistory
        {
            get { return _deleteHistory; }
            protected set { SetProperty(ref _deleteHistory, value); }
        }

        private bool _isLogEnabled;
        public bool IsLogEnabled
        {
            get { return _isLogEnabled; }
            set
            {
                SetProperty(ref _isLogEnabled, value);
                UpdateLogSetting(_isLogEnabled);
            }
        }

        private void UpdateLogSetting(bool isLogEnabled)
        {
            var session = _sessionRepository.GetSessionObject();
            session.RegisterLog = isLogEnabled;
            _sessionRepository.Update(session);
            MessagingCenter.Send(new PersistLogMessage(isLogEnabled), PersistLogMessage.Name);
        }
        private void ForceLogGeneration()
        {
            var varEnableLogGlobally = 0;
            if (!int.TryParse(Parameters.Get(ParameterCode.EnableLogGlobally), out varEnableLogGlobally))
                varEnableLogGlobally = 0;
            if (varEnableLogGlobally == 0) return;

            IsLogEnabled = varEnableLogGlobally == 1; //If its enabled, then set the new value and update the view
        }
    }
}
