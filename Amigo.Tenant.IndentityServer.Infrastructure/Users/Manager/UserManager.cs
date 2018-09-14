using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager
{
    public class UserManager : UserManager<User,int>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            this.ClaimsIdentityFactory = new ClaimsFactory();

            var provider = new DpapiDataProtectionProvider("Amigo.Tenant.Identity");

            this.UserTokenProvider = new DataProtectorTokenProvider<User, int>(provider.Create("PasswordReset"));
        }
    }
}