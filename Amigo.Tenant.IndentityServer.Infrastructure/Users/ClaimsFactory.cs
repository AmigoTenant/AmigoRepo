using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users
{
    public class ClaimsFactory : ClaimsIdentityFactory<User, int>
    {
        public ClaimsFactory()
        {
            this.UserIdClaimType = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            this.UserNameClaimType = IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName;
            this.RoleClaimType = IdentityServer3.Core.Constants.ClaimTypes.Role;
        }

        public override async System.Threading.Tasks.Task<System.Security.Claims.ClaimsIdentity> CreateAsync(UserManager<User, int> manager, User user, string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);
            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                ci.AddClaim(new Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                ci.AddClaim(new Claim("family_name", user.LastName));
            }
            return ci;
        }
    }
}