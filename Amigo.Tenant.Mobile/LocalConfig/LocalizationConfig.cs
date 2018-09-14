using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XPO.ShuttleTracking.Mobile.Infrastructure;
using XPO.ShuttleTracking.Mobile.Resource;

namespace XPO.ShuttleTracking.Mobile.LocalConfig
{
    public class LocalizationConfig : BindableObject
    {
        //private readonly IPersistentStorageManager _persistentStorageManager;
        private static readonly string[] SupportedLanguages = { "en-US", "es-PE" };

        private int _cultureIndex;
        private ObservableCollection<string> _languages;

        public LocalizationConfig()
        {
            
        }
        //public LocalizationConfig(IPersistentStorageManager persistentStorageManager)
        //{
        //    _persistentStorageManager = persistentStorageManager;
        //}

        public string GetCurrentCulture()
        {
            return SupportedLanguages[_cultureIndex];
        }

        public int SelectedLanguage
        {
            get { return _cultureIndex; }
            set
            {
                if (_cultureIndex == value) return;
                _cultureIndex = value;

                OnPropertyChanged();

                //_persistentStorageManager.AddValue(AppSettings.Language, _cultureIndex);

                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                    AppString.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo(GetCurrentCulture());

                LoadLanguagesList();

                //MessagingCenter.Send(new CultureChangedMessage(GetCurrentCulture()), CultureChangedMessage.Name);
            }
        }

        public ObservableCollection<string> Languages => _languages;

        public void InitLanguage()
        {
            //var currentCulture = _persistentStorageManager.ReadValue<int>(AppSettings.Language);
            //_cultureIndex = currentCulture;
            _cultureIndex = 0;
            LoadLanguagesList();

            Current = this;
        }

        public static LocalizationConfig Current { get; private set; }
        private void LoadLanguagesList()
        {
            string rbtLanguage = "English|Spanish";
            var values = rbtLanguage.Split('|').ToList();
            _languages = new ObservableCollection<string>(values);
            OnPropertyChanged("Languages");
        }
    }
}
