using Microsoft.AspNet.Identity;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager
{
    public class RoleManager : RoleManager<Role,int>
    {

        public RoleManager(RoleStore store):base(store)
        {            
        }        
    }
}