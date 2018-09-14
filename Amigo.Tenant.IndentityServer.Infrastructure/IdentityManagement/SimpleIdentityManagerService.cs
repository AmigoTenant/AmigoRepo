using IdentityManager.AspNetIdentity;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.IdentityManagement
{
    public class SimpleIdentityManagerService : AspNetIdentityManagerService<User, int, Role, int>
    {
        public SimpleIdentityManagerService(UserManager userMgr, RoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }
}