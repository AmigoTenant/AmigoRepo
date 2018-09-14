using System;
using System.Configuration;

namespace Amigo.Tenant.IdentityServer.Configuration
{
    public static class Settings
    {
        private const string EnableUsersKey      ="UsersAdmin.Enable";
        private const string EnableClientsKey    ="ClientsAdmin.Enable";
        

        private const string WinDSStoreNameKey      ="Windows.DSDomain";
        private const string WinEmailDomainKey      ="Windows.EmailDomain";

        private const string PublicUrlkey           ="Identity.PublicUrl";
        private const string UsersApiScopeKey       ="Identity.ServiceScope";
        private const string SiteNameKey            ="Identity.SiteName";
        private const string IdentityTraceEnabledKey = "Identity.TraceEnabled";

        private const string UsersServiceSecurityKey = "UsersService.SecurityEnabled";

        private const string RaygunApiKeyKey = "Raygun.Apikey";
        private const string RaygunTagKey = "Raygun.Tag";


        public static bool GetBoolSetting(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var raw = ConfigurationManager.AppSettings[key];
            if(raw==null) throw new InvalidOperationException("There is no key in App Settings.");

            return bool.Parse(raw);
        }

        public static string GetSetting(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var raw = ConfigurationManager.AppSettings[key];
            if (raw == null) throw new InvalidOperationException("There is no key in App Settings.");

            return raw;
        }

        public static bool IsUserAdministrationEnable => GetBoolSetting(EnableUsersKey);
        public static bool IsClientAdministrationEnable => GetBoolSetting(EnableClientsKey);        
        public static string WindowsDSStoreName => GetSetting(WinDSStoreNameKey);
        public static string WindowsEmailDomain => GetSetting(WinEmailDomainKey);
        public static string PublicUrl => GetSetting(PublicUrlkey);
        public static string UsersApiScope => GetSetting(UsersApiScopeKey);
        public static bool UsersServiceSecurityEnabled => GetBoolSetting(UsersServiceSecurityKey);

        public static string RaygunApikey => GetSetting(RaygunApiKeyKey);
        public static string RaygunTag => GetSetting(RaygunTagKey);
        public static string SiteName => GetSetting(SiteNameKey);

        public static bool TraceEnabled => GetBoolSetting(IdentityTraceEnabledKey);
    }
}