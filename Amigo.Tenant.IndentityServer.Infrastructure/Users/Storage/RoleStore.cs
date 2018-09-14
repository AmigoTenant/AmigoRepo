using Microsoft.AspNet.Identity.EntityFramework;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;
using IdentityUserRole = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserRole;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage
{
    public class RoleStore : RoleStore<Role, int, IdentityUserRole>
    {

        public RoleStore(UsersDbContext context)
          : base(context)
        {
        }
    }
}