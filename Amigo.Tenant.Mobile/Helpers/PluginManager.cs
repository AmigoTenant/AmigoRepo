using System;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace XPO.ShuttleTracking.Mobile.Helpers
{
    public class PluginManager
    {
        static PluginManager()
        {
            Geolocator = CrossGeolocator.Current;
        }

        public static void Init(IGeolocator geolocator)
        {
            if (geolocator == null) throw new ArgumentNullException(nameof(geolocator));
            Geolocator = geolocator;
        }

        public static IGeolocator Geolocator { get; private set; }        
    }
}
