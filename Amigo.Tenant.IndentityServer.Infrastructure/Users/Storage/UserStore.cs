using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;
using IdentityUserClaim = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserClaim;
using IdentityUserLogin = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserLogin;
using IdentityUserRole = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserRole;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage
{
    public class UserStore : UserStore<User, Role, int, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(UsersDbContext ctx)
            : base(ctx)
        {

        }

        public Task<List<User>> FindUsersAsync(Expression<Func<User, bool>> filter)
        {
            return this.Context.Set<User>().Where(filter).ToListAsync();
        }

        public Task<List<User>> FindUsersAsync(Expression<Func<User, bool>> filter,int page,int pagesize)
        {
            return this.Context.Set<User>().OrderBy(x=> x.Id).Where(filter).Skip(pagesize*(page-1)).Take(pagesize).ToListAsync();
        }

        public Task<int> CountUsersAsync(Expression<Func<User, bool>> filter)
        {
            return this.Context.Set<User>().Where(filter).CountAsync();
        }

    }
}