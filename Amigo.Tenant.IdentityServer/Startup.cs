using System.Web.Http;
using IdentityAdmin.Configuration;
using IdentityManager.Configuration;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using NLog.Owin.Logging;
using Owin;
using Serilog;
using Amigo.Tenant.IdentityServer.Configuration;
using Amigo.Tenant.IdentityServer.Infrastructure.Certificate;
using Amigo.Tenant.IdentityServer.Infrastructure.ClientManagement;
using Amigo.Tenant.IdentityServer.Infrastructure.IdentityManagement;
using Amigo.Tenant.IdentityServer.Infrastructure.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Storage.EntityFramework;
using Amigo.Tenant.IdentityServer.Infrastructure.ViewService;
using Amigo.Tenant.IdentityServer.Infrastructure.ViewService.Extensions;
using EmbeddedAssetsViewLoader = Amigo.Tenant.IdentityServer.Infrastructure.ViewService.EmbeddedAssetsViewLoader;
using IViewLoader = Amigo.Tenant.IdentityServer.Infrastructure.ViewService.IViewLoader;
using Swashbuckle.Application;
using System.Linq;

[assembly: OwinStartup(typeof(Amigo.Tenant.IdentityServer.Startup))]

namespace Amigo.Tenant.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #region Identity Server

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo
                //.Raygun(Settings.RaygunApikey, tags: new[] { Settings.RaygunTag })
                .LiterateConsole()
                .CreateLogger();

            if (Settings.TraceEnabled)
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo
                .Trace()
                .CreateLogger();
            }

            var fact = Factory.Configure(Constants.ConnectionName, Settings.WindowsDSStoreName, Settings.WindowsEmailDomain);
            fact.Register(new IdentityServer3.Core.Configuration.Registration<IViewLoader, EmbeddedAssetsViewLoader>());
            fact.ViewService = new Infrastructure.ViewService.DefaultViewServiceRegistration<AmigoTenantViewService>();
            //Identity Server
            var options = new IdentityServerOptions
            {
                Factory = fact,
                RequireSsl = false,
                SigningCertificate = Certificate.Load(),
                EnableWelcomePage = false,
                PublicOrigin = Settings.PublicUrl,
                SiteName = Settings.SiteName,
                AuthenticationOptions = new AuthenticationOptions()
                {
                    EnablePostSignOutAutoRedirect = true
                }
            };

            app.UseIdentityServer(options);
            #endregion

            #region Identity Management
            if (Settings.IsUserAdministrationEnable)
            {
                var factory = new IdentityManagerServiceFactory();
                factory.ConfigureSimpleIdentityManagerService(Constants.ConnectionName);

                var opt = new IdentityManagerOptions
                {
                    Factory = factory,
                    SecurityConfiguration = { RequireSsl = false }
                };

                app.Map("/admin/users", _ => _.UseIdentityManager(opt));
            }
            #endregion

            #region Identity Admin
            if (Settings.IsClientAdministrationEnable)
            {
                app.Map("/admin/clients", adminApp =>
                {
                    var fct = new IdentityAdminServiceFactory();
                    fct.Configure();
                    adminApp.UseIdentityAdmin(new IdentityAdminOptions
                    {
                        Factory = fct,
                        AdminSecurityConfiguration = { RequireSsl = false }
                    });
                });
            }
            #endregion

            #region WebApi Services                        

            var config = new HttpConfiguration();

            app.UseNLog();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = Settings.PublicUrl,
                RequiredScopes = new[] { Settings.UsersApiScope },
                EnableValidationResultCache = true,
                ValidationMode = ValidationMode.Local,
                DelayLoadMetadata = true
            });            

            DependencyInjectionConfig.Configure(config);
            WebApiConfig.Register(config, Settings.UsersServiceSecurityEnabled);

            app.UseWebApi(config);            

            config.EnableSwagger(c => {
                c.SingleApiVersion("v1", "Amigo Tenant Services");
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            })
                  .EnableSwaggerUi();

            app.UseAmigoTenantEmbeddedFileServer();

            #endregion
        }
    }
}