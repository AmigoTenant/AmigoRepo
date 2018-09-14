using IdentityManager;
using IdentityManager.Configuration;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;

namespace Amigo.Tenant.IdentityServer.Infrastructure.IdentityManagement
{
    public static class SimpleIdentityManagerServiceExtensions
    {
        public static void ConfigureSimpleIdentityManagerService(this IdentityManagerServiceFactory factory, string connectionString)
        {
            factory.Register(new Registration<UsersDbContext>(resolver => new UsersDbContext(connectionString)));
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<RoleStore>());
            factory.Register(new Registration<UserManager>());
            factory.Register(new Registration<RoleManager>());
            factory.IdentityManagerService = new Registration<IIdentityManagerService, SimpleIdentityManagerService>();
        }
    }
}