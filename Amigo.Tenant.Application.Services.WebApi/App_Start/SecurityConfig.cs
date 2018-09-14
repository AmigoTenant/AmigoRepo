using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Principal;
using IdentityServer3.AccessTokenValidation;
using Owin;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Configuration;
using System.Security.Claims;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class SecurityConfig
    {
        public static void UseSecurity(this IAppBuilder app)
        {
            ////Security
            if (Settings.SecurityEnabled)
            {
                app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = Settings.SecurityAuthority,                    
                    RequiredScopes = new[] { "XST.Services" }
                });
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    var identity = new GenericIdentity("root", "Test User for disabled security");                    
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "3015"));//user ID to be used in the audit fields
                    context.Request.User = new GenericPrincipal(identity, new[] { "XST.Admin" });
                    await next();
                });
            }
        }
    }
}