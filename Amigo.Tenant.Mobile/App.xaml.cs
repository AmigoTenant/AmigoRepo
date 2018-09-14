using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PCLAppConfig;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using TSI.Xamarin.Forms.Logging.Abstract;
using TSI.Xamarin.Forms.Mvvm.Bootstraping;
using TSI.Xamarin.Forms.Mvvm.Views.Abstract;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.DependencyInjection;
using XPO.ShuttleTracking.Mobile.Domain.Resources;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.GeofenceService.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence.NoSql.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Presentation.Messaging;
using XPO.ShuttleTracking.Mobile.Infrastructure.Resources;
using XPO.ShuttleTracking.Mobile.Infrastructure.RootValidator;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.Services;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile
{
    public partial class App
    {
        public readonly IResolver Resolver;
        private readonly Bootstrapper _bootstrapper;
        public App(IResolver resolver)
        {
            Logger.Current.LogInfo($"Shuttle Tracking Start");
            Resolver = resolver;
            InitializeComponent();
            _bootstrapper = new Bootstrapper(this,resolver);
            InitializeLocalization();
        }

        protected override void OnStart()
        {
            InitializeConfigurationManager();            
            InitializeTaskManager();
            InitializeLog();
            InitializeGeolocationPin();

            _bootstrapper.Run();

            //Add MainMenu to the stack
            var navigationPage = MainPage as NavigationPage;
            if (_bootstrapper.ShouldAddMainMenu && navigationPage!= null)
            {
                var resolver = Resolver.Resolve<IViewFactory>();
                MainMenuViewModel mainvm;
                var mainPage = resolver.Resolve<MainMenuViewModel>(out mainvm);
                mainvm.OnPushed();
                var movePage = navigationPage.CurrentPage;
                MainPage.Navigation.InsertPageBefore(mainPage,movePage);
            }

            // Handle when your app starts
            Task.Run(() =>
            {                
                ValidationRoot();
            }).ConfigureAwait(false);
        }

        private void InitializeLog()
        {
            var logger = Resolver.Resolve<ILogger>();
            var sessionRepository = Resolver.Resolve<ISessionRepository>();
            var session = sessionRepository.GetSessionObject();
            MessagingCenter.Send(new PersistLogMessage(session.RegisterLog),PersistLogMessage.Name);
        }

        private void InitializeTaskManager()
        {
            if (TaskManager.Current == null)
            {
                var task = Resolver.Resolve<TaskManager>();
                TaskManager.InitTaskManager(task);
            }
        }
        private void InitializeGeolocationPin()
        {
            if (ShuttleTrackingGeofenceService.Current == null)
            {
                var geofenceService = Resolver.Resolve<IGeofenceService>();
                ShuttleTrackingGeofenceService.Init(geofenceService);

            }
        }

        private void InitializeLocalization()
        {
            if(AppString.Culture != null) return;
            //var localizationConfig = _resolver.Resolve<LocalizationConfig>();                        

            //var current = localizationConfig.GetCurrentCulture();            

            AppString.Culture = new System.Globalization.CultureInfo("en");
            MessagesResources.Culture = new System.Globalization.CultureInfo("en");
            StringResources.Culture = new System.Globalization.CultureInfo("en");
            //;DependencyService.Get<XPO.ShuttleTracking.Mobile.Infrastructure.ILocalize>().GetCurrentCultureInfo(current);
        }

        private static void InitializeConfigurationManager()
        {
            if (ConfigurationManager.AppSettings == null)
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("XPO.ShuttleTracking.Mobile.app.config");
                ConfigurationManager.Initialise(stream);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            CrossConnectivity.Current.ConnectivityChanged -= AskConnectivityChanged;            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CrossConnectivity.Current.ConnectivityChanged += AskConnectivityChanged;
        }

        private void ValidationRoot()
        {
            var rootValidation = Resolver.Resolve<IRootValidator>();
            if (rootValidation.isBroken())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await MessageService.Current.ShowToastAsync("Jailbroken device!");
                });
            }
        }

        private void UpdateInternetStatus(bool state)
        {
            var conectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
            string connectionName;
            switch (conectionType)
            {
                case ConnectionType.Wimax:
                case ConnectionType.Bluetooth:
                case ConnectionType.Cellular:
                    connectionName = ConnectionTypeName.gps;
                    break;
                default:
                    connectionName = ConnectionTypeName.network;
                    break;
            }
            SessionParameter.IsConnectivity = state;
            SessionParameter.LocationProvider = connectionName;
        }

        private void AskConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            UpdateInternetStatus(e.IsConnected);
        }

        private bool _shouldAddMainMenu = false;
    }
}
