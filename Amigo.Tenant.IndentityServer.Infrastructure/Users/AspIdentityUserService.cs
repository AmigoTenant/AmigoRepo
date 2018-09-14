using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Amigo.Tenant.IdentityServer.Infrastructure.App_Packages.IdentityServer3.AspNetIdentity;
using Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users
{
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString,string dsstorename,string emaildomain)
        {
            factory.Register(new Registration<WindowsUserValidator>(resolver => new WindowsUserValidator(dsstorename,emaildomain)));
            factory.UserService = new Registration<IUserService,WindowsUserService>();
            factory.Register(new Registration<UserManager>());
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<UsersDbContext>(resolver => new UsersDbContext(connString)));
        }
    }

    public class UserService : AspNetIdentityUserService<User,int>
    {
        public UserService(UserManager userMgr)
            : base(userMgr)
        {
        }

        protected override async Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsFromAccount(User user)
        {
            var claims = (await base.GetClaimsFromAccount(user)).ToList();
            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                claims.Add(new System.Security.Claims.Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                claims.Add(new System.Security.Claims.Claim("family_name", user.LastName));
            }

            return claims;
        }
    }
}