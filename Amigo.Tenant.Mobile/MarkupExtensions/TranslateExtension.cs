using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.LocalConfig;

namespace XPO.ShuttleTracking.Mobile.MarkupExtensions
{
    [ContentProperty("Text")]
    class TranslateExtension : IMarkupExtension, IDisposable
    {
        private CultureInfo _ci;
        private const string ResourceId = "XPO.ShuttleTracking.Mobile.Resource.AppString";

        public TranslateExtension()
        {
            _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo(LocalizationConfig.Current.GetCurrentCulture());

            //MessagingCenter.Subscribe<CultureChangedMessage>(this, CultureChangedMessage.Name, OnCultureChange);
        }

        private void OnCultureChange(CultureChangedMessage cultureChangeMessage)
        {
            _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo(cultureChangeMessage.Culture);
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, _ci);

            if (translation == null)
            {
                #if DEBUG
                    throw new ArgumentException($"Key '{Text}' was not found in resources '{ResourceId}' for culture '{_ci.Name}'.", nameof(serviceProvider));
                #else
                    translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
                #endif
            }
            return translation;
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<CultureChangedMessage>(this, CultureChangedMessage.Name);
        }
    }
}
