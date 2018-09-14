using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Tasks.Data.DefinitionAbstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.BackgroundTasks;
using XPO.ShuttleTracking.Mobile.Infrastructure.GeofenceService.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Helpers.Abstract;
using XPO.ShuttleTracking.Mobile.Infrastructure.Persistence;

namespace XPO.ShuttleTracking.Mobile.Services
{
    public class ShuttleTrackingGeofenceService : IGeofenceService, IDisposable
    {
        private bool _geolocationInProcess;
        private readonly IWebServiceCallingInfomationProvider _infomationProvider;

        public volatile bool IsRunning = false;

        public ShuttleTrackingGeofenceService(
            IWebServiceCallingInfomationProvider infomationProvider)
        {
            _infomationProvider = infomationProvider;

            _lazyParameterInterval = new Lazy<int>(GetIntervalPing);

            CrossGeolocator.Current.PositionChanged += Locator_PositionChanged;
        }

        private readonly Lazy<int> _lazyParameterInterval;
        private static CancellationTokenSource _cancellationTokenSource;

        public int ParameterInterval => _lazyParameterInterval.Value;

        private int GetIntervalPing()
        {
            int intervalPing;

            var parameter = Parameters.Get(ParameterCode.GeolocPingInterval);

            if (!int.TryParse(parameter, out intervalPing)) intervalPing = 2;

            return intervalPing <= 0 ? 2 : intervalPing;
        }

        public void Stop()
        {
            StopListeningGps();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;

            IsRunning = false;
        }

        public static IGeofenceService Current { get; private set; }

        public static void Init(IGeofenceService geofenceService)
        {
            Current = geofenceService;
        }

        public void Start()
        {               
            if(IsRunning)return;

            IsRunning = true;

            try
            {
                if (_cancellationTokenSource == null) _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        StartListeningGps();

                        await Task.Delay(TimeSpan.FromSeconds(30),token);

                        while (!token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                            ProcessGeolocation();
                            await Task.Delay(TimeSpan.FromMinutes(ParameterInterval), token);
                        }
                    }
                    catch (TaskCanceledException tex)
                    {
                        Debug.WriteLine($"Geolocation cancelled:{tex}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    finally
                    {
                        IsRunning = false;
                    }
                }, _cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }           
        }        

        private void ProcessGeolocation()
        {
            if (_geolocationInProcess) return;
            _geolocationInProcess = true;

            var request = new RegisterLogTaskDefinition
            {
                ActivityCode = ActivityCode.GeolocationPing,
                ChargeNo = SessionParameter.CurrentActivityChargeNo
            };
            _infomationProvider.FillTaskDefinition(ref request);
            TaskManager.Current.RegisterStoreAndForward(request);
            _geolocationInProcess = false;
        }

        public void StartListeningGps()
        {
            if (!CrossGeolocator.Current.IsListening) CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMilliseconds(100), 1, includeHeading: true);

            if (!CrossGeolocator.Current.IsGeolocationAvailable || !CrossGeolocator.Current.IsGeolocationEnabled) return;
            
            CrossGeolocator.Current.DesiredAccuracy = Common.Constants.Geofence.DESIRED_ACCURACY;            
        }

        public void StopListeningGps()
        {
            CrossGeolocator.Current.StopListeningAsync();
        }

        public void Locator_PositionChanged(object sender, PositionEventArgs o)
        {
            try
            {
                SessionParameter.Latitude = (decimal)o.Position.Latitude;
                SessionParameter.Longitude = (decimal)o.Position.Longitude;
                SessionParameter.Accuracy = (int)o.Position.Accuracy;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Locator Position Change : {ex}");
            }
        }

        public void Dispose()
        {
            CrossGeolocator.Current.PositionChanged -= Locator_PositionChanged;
            StopListeningGps();
        }
    }
}