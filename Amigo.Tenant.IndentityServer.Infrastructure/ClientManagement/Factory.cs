using IdentityAdmin.Configuration;
using IdentityAdmin.Core;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ClientManagement
{    
    public static class IdentityAdminServiceExtensions
    {
        public static void Configure(this IdentityAdminServiceFactory factory)
        {            
            factory.IdentityAdminService = new IdentityAdmin.Configuration.Registration<IIdentityAdminService, IdentityAdminManagerService>();
        }
    }
}