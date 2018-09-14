using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration
{
    public static class Settings
    {
        private const string SecurityEnabledKey = "security.enabled";
        private const string SecurityAuthorityKey = "security.authority";
        private const string CorsDomainKey = "cors.domains";
        private const string ServicesRootUrlKey = "services.rootUrl";
        private const string RaygunTagKey = "raygun.Tag";
        private const string IdentityServerClientIdKey = "identityServer.clientId";
        private const string IdentityServerClientSecretKey = "identityServer.clientSecret";
        private const string IdentityServerClientScopeKey = "identityServer.clientScope";
        private const string RequestLogEnabledKey = "requestLog.enabled";

        public static bool SecurityEnabled => bool.Parse(ConfigurationManager.AppSettings[SecurityEnabledKey]);
        public static string SecurityAuthority => ConfigurationManager.AppSettings[SecurityAuthorityKey];
        public static string CorsDomain => ConfigurationManager.AppSettings[CorsDomainKey];
        public static string RootUrl => ConfigurationManager.AppSettings[ServicesRootUrlKey];
        public static string RaygunTag => ConfigurationManager.AppSettings[RaygunTagKey];
        public static string IdentityServerClientId => ConfigurationManager.AppSettings[IdentityServerClientIdKey];
        public static string IdentityServerClientSecret => ConfigurationManager.AppSettings[IdentityServerClientSecretKey];
        public static string IdentityServerClientScope => ConfigurationManager.AppSettings[IdentityServerClientScopeKey];
        public static bool RequestLogEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings[RequestLogEnabledKey]);

    }
}