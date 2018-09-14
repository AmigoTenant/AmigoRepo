using System.Collections.Generic;
using IdentityServer3.Core;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for the OWIN environment.
    /// </summary>
    public static class OwinEnvironmentExtensions
    {
        /// <summary>
        /// Gets the public host name for IdentityServer.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <returns></returns>
        public static string GetIdentityServerHost(this IDictionary<string, object> env)
        {
            return env[Constants.OwinEnvironment.IdentityServerHost] as string;
        }

        /// <summary>
        /// Gets the base path of IdentityServer. Can be used inside of Katana <c>Map</c>ped middleware.
        /// </summary>
        /// <param name="env">The OWIN environment.</param>
        /// <returns></returns>
        public static string GetIdentityServerBasePath(this IDictionary<string, object> env)
        {
            return env[Constants.OwinEnvironment.IdentityServerBasePath] as string;
        }

        /// <summary>
        /// Gets the public base URL for IdentityServer.
        /// </summary>
        /// <param name="env">The OWIN environment.</param>
        /// <returns></returns>
        public static string GetIdentityServerBaseUrl(this IDictionary<string, object> env)
        {
            return env.GetIdentityServerHost() + env.GetIdentityServerBasePath();
        }

        /// <summary>
        /// Gets the URL for the logout page.
        /// </summary>
        /// <param name="env">The OWIN environment.</param>
        /// <returns></returns>
        public static string GetIdentityServerLogoutUrl(this IDictionary<string, object> env)
        {
            return env.GetIdentityServerBaseUrl() + Constants.RoutePaths.Logout;
        }
    }
}
